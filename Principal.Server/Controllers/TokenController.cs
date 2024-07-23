#define SEGURIDAD

using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Principal.Server.Guards;
using Principal.Server.Objects;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Principal.Server.Controllers
{
#if SEGURIDAD
    [Authorize]
#endif
    public class TokenController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            //RequestValidator.ValidateAppRole("access_as_application");

            return Ok(new RequestSecretModel()
            {
                Aws_Secret_Id = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID", EnvironmentVariableTarget.User),
                Aws_Secret_Key = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", EnvironmentVariableTarget.User),
            });
        }

        [HttpPost]
        [Route("GetSecret")]
        public async Task<IHttpActionResult> Get([FromBody] RequestSecretModel requestSecret)
        {
            try
            {
                var awsCredentials = new BasicAWSCredentials(requestSecret.Aws_Secret_Id, requestSecret.Aws_Secret_Key);
                using (var awsClient = new AmazonSecretsManagerClient(awsCredentials, RegionEndpoint.USEast2))
                {
                    var secretRequest = new GetSecretValueRequest
                    {
                        SecretId = requestSecret.SecretName,
                        VersionStage = "AWSCURRENT"
                    };

                    var secretResponse = await awsClient.GetSecretValueAsync(secretRequest);
                    var secretString = secretResponse.SecretString;

                    return Ok(secretString);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
