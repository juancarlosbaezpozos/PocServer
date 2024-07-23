using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using Principal.Server.Objects;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Principal.Server.Processors
{
    public class RotationKeyProcessor : StiProcessor
    {
        protected override TimeSpan SuspendTime { get; } = TimeSpan.FromDays(1);

        public override string Name => "RotationKeyProcessor";

        readonly string ACCESS_ID;
        readonly string ACCESS_SECRET;

        public RotationKeyProcessor(IStiCore core, int? processorIndex)
            : base(core, processorIndex)
        {
            //Llaves maestras
            ACCESS_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID", EnvironmentVariableTarget.User);
            ACCESS_SECRET = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", EnvironmentVariableTarget.User);
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
            var expiryThreshold = 90;
            var deactivationThreshold = 70;

            var awsCredentialsA = new BasicAWSCredentials(ACCESS_ID, ACCESS_SECRET);
            using (var iamClient = new AmazonIdentityManagementServiceClient(awsCredentialsA, RegionEndpoint.USEast2))
            {
                var usersResponse = await iamClient.ListUsersAsync();
                foreach (var user in usersResponse.Users.Where(u => u.UserName == "usera"))
                {
                    var userName = user.UserName;
                    var accessKeyRequst = new ListAccessKeysRequest()
                    {
                        UserName = userName
                    };

                    var accessKeyDetails = await iamClient.ListAccessKeysAsync(accessKeyRequst);
                    foreach (var accessKeyx in accessKeyDetails.AccessKeyMetadata)
                    {
                        var createdDate = accessKeyx.CreateDate;
                        var keyAge = DateTime.Now - createdDate;
                        var keyAgeInDays = keyAge.Days;
                        if (keyAgeInDays >= deactivationThreshold - 10
                        && keyAgeInDays < deactivationThreshold
                        && accessKeyDetails.AccessKeyMetadata.Count == 1
                        && accessKeyx.Status == StatusType.Active)
                        {
                            await CreateAccessKey(userName);
                        }
                        else if (keyAgeInDays >= expiryThreshold - 10
                        && keyAgeInDays < expiryThreshold
                        && accessKeyx.Status == StatusType.Active)
                        {
                            await DeactivateAccessKey(userName, accessKeyx.AccessKeyId);
                        }
                        else if (keyAgeInDays >= expiryThreshold && accessKeyx.Status == StatusType.Inactive)
                        {
                            await DeleteAccessKey(userName, accessKeyx.AccessKeyId);
                        }
                        else
                        {
                            //TODO: recuperar las actuales
                        }
                    }
                }
            }
        }

        private async Task CreateAccessKey(string userName)
        {
            var awsCredentialsA = new BasicAWSCredentials(ACCESS_ID, ACCESS_SECRET);
            using (var iamClient = new AmazonIdentityManagementServiceClient(awsCredentialsA, RegionEndpoint.USEast2))
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
            using (var iamClient = new AmazonIdentityManagementServiceClient(awsCredentialsA, RegionEndpoint.USEast2))
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
            using (var iamClient = new AmazonIdentityManagementServiceClient(awsCredentialsA, RegionEndpoint.USEast2))
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

        private void UpdateEnv(string id, string key)
        {
            Environment.SetEnvironmentVariable("LTD_USR_ACS_ID", id, EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("LTD_USR_ACS_KEY", key, EnvironmentVariableTarget.User);
        }

    }
}
