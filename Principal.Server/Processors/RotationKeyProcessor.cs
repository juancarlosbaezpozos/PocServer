using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using Principal.Server.Objects;
using System;
using System.Threading.Tasks;

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
                foreach (var user in usersResponse.Users)
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
                        && accessKeyx.Status == Amazon.IdentityManagement.StatusType.Active)
                        {
                            await CreateAccessKey(user.UserName);
                        }
                        else if (keyAgeInDays >= expiryThreshold - 10
                        && keyAgeInDays < expiryThreshold
                        && accessKeyx.Status == Amazon.IdentityManagement.StatusType.Active)
                        {
                            await DeactivateAccessKey(user.UserName, accessKeyx.AccessKeyId);
                        }
                        else if (keyAgeInDays >= expiryThreshold && accessKeyx.Status == Amazon.IdentityManagement.StatusType.Inactive)
                        {
                            await DeleteAccessKey(user.UserName, accessKeyx.AccessKeyId);
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
                var message = $"New Access Key Generated for user : {userName}. " +
                $"New Access Key Id : {keyDetails.AccessKey.AccessKeyId}. " +
                $"New Secret Access Key : {keyDetails.AccessKey.SecretAccessKey}." +
                $"Old Key will be Deactivated in about 10 days.";
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
                var message = $"Access Key : {accessKeyId} has been deleted for user : {userName}.";
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
                var message = $"Access Key : {accessKeyId} has been deactivated for user : {userName}.";
            }
        }

    }
}
