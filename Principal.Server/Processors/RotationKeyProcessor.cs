using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Principal.Server.Objects;
using System;
using System.Threading.Tasks;

namespace Principal.Server.Processors
{
    public class RotationKeyProcessor : StiProcessor
    {
        protected override TimeSpan SuspendTime { get; } = TimeSpan.FromDays(1);

        public override string Name => "RotationKeyProcessor";

        private readonly string ACCESS_ID;
        private readonly string ACCESS_SECRET;
        private readonly string USER_MONITOR;
        private readonly RegionEndpoint REGIONPOINT;
        private readonly short ROTATIONDAYS;
        private readonly short DEACTIVATIONDAYS;

        public RotationKeyProcessor(IStiCore core, int? processorIndex, IConfiguration configuration)
            : base(core, processorIndex)
        {
            ACCESS_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID", EnvironmentVariableTarget.User);
            ACCESS_SECRET = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", EnvironmentVariableTarget.User);
            USER_MONITOR = configuration["Aws:UserMonitor"];
            REGIONPOINT = RegionEndpoint.GetBySystemName(configuration["Aws:RegionName"]) ?? RegionEndpoint.USEast2;
            ROTATIONDAYS = Convert.ToInt16(configuration["Aws:RotationDays"]);
            DEACTIVATIONDAYS = Convert.ToInt16(configuration["Aws:DeactivationDays"]);
        }

        protected override void Process()
        {
            try
            {
                Task.Run(async () => await ProcesarRotacion()).Wait();
            }
            catch
            {
            }
        }

        protected async Task ProcesarRotacion()
        {
            if (string.IsNullOrEmpty(ACCESS_ID)) return;
            if (string.IsNullOrEmpty(ACCESS_SECRET)) return;
            if (string.IsNullOrEmpty(USER_MONITOR)) return;
            if (ROTATIONDAYS < 15) return;
            if (DEACTIVATIONDAYS < 15) return;

            var awsCredentialsA = new BasicAWSCredentials(ACCESS_ID, ACCESS_SECRET);
            using (var iamClient = new AmazonIdentityManagementServiceClient(awsCredentialsA, REGIONPOINT))
            {
                var usersResponse = await iamClient.ListUsersAsync();
                foreach (var user in usersResponse.Users)
                {
                    var userName = user.UserName;
                    if (userName != USER_MONITOR)
                        continue;

                    var accessKeyRequst = new ListAccessKeysRequest()
                    {
                        UserName = userName
                    };

                    var accessKeyDetails = await iamClient.ListAccessKeysAsync(accessKeyRequst);
                    if (accessKeyDetails.AccessKeyMetadata.Count == 0)
                    {
                        await CreateAccessKey(userName);
                    }

                    foreach (var accessKeyx in accessKeyDetails.AccessKeyMetadata)
                    {
                        var createdDate = accessKeyx.CreateDate;
                        var keyAge = DateTime.Now - createdDate;
                        var keyAgeInDays = keyAge.Days;

                        if (keyAgeInDays >= DEACTIVATIONDAYS - 10
                        && keyAgeInDays < DEACTIVATIONDAYS
                        && accessKeyDetails.AccessKeyMetadata.Count == 1
                        && accessKeyx.Status == StatusType.Active)
                        {
                            await CreateAccessKey(userName);
                        }
                        else if (keyAgeInDays >= ROTATIONDAYS - 10
                        && keyAgeInDays < ROTATIONDAYS
                        && accessKeyx.Status == StatusType.Active)
                        {
                            await DeactivateAccessKey(userName, accessKeyx.AccessKeyId);
                        }
                        else if (keyAgeInDays >= ROTATIONDAYS && accessKeyx.Status == StatusType.Inactive)
                        {
                            await DeleteAccessKey(userName, accessKeyx.AccessKeyId);
                        }
                    }
                }
            }
        }

        private async Task CreateAccessKey(string userName)
        {
            var awsCredentialsA = new BasicAWSCredentials(ACCESS_ID, ACCESS_SECRET);
            using (var iamClient = new AmazonIdentityManagementServiceClient(awsCredentialsA, REGIONPOINT))
            {
                var request = new CreateAccessKeyRequest()
                {
                    UserName = userName
                };

                var keyDetails = await iamClient.CreateAccessKeyAsync(request);
                UpdateEnv(keyDetails.AccessKey.AccessKeyId, keyDetails.AccessKey.SecretAccessKey);
            }
        }

        private async Task DeleteAccessKey(string userName, string accessKeyId)
        {
            var awsCredentialsA = new BasicAWSCredentials(ACCESS_ID, ACCESS_SECRET);
            using (var iamClient = new AmazonIdentityManagementServiceClient(awsCredentialsA, REGIONPOINT))
            {
                var request = new DeleteAccessKeyRequest()
                {
                    UserName = userName,
                    AccessKeyId = accessKeyId
                };

                await iamClient.DeleteAccessKeyAsync(request);
                UpdateEnv("", "");
            }
        }

        private async Task DeactivateAccessKey(string userName, string accessKeyId)
        {
            var awsCredentialsA = new BasicAWSCredentials(ACCESS_ID, ACCESS_SECRET);
            using (var iamClient = new AmazonIdentityManagementServiceClient(awsCredentialsA, REGIONPOINT))
            {
                var request = new UpdateAccessKeyRequest()
                {
                    UserName = userName,
                    AccessKeyId = accessKeyId,
                    Status = StatusType.Inactive
                };

                await iamClient.UpdateAccessKeyAsync(request);
                UpdateEnv("", "");
            }
        }

        private static void UpdateEnv(string id, string key)
        {
            Environment.SetEnvironmentVariable("LTD_USR_ACS_ID", id, EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("LTD_USR_ACS_KEY", key, EnvironmentVariableTarget.User);
        }
    }
}
