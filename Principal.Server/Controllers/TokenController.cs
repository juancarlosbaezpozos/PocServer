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

            return Ok("token_demo");
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
                    var secretRequest = new GetSecretValueRequest();
                    secretRequest.SecretId = requestSecret.SecretName;
                    secretRequest.VersionStage = "AWSCURRENT";

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
