using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
#if SEGURIDAD
using Principal.Server.Guards;
#endif
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
        [HttpPost]
        [Route("GetSecret")]
        public async Task<IHttpActionResult> Get([FromBody] RequestSecretModel requestSecret)
        {
            try
            {
#if SEGURIDAD
                var serviceName = RequestValidator.ValidateClaimsPrincipal();
                System.Diagnostics.Debug.WriteLine($"Petición desde: {serviceName}");

                //if (serviceName != "Cancelacion")
                //    return Unauthorized();
#endif

                var Aws_Secret_Id =
                    Environment.GetEnvironmentVariable("LTD_USR_ACS_ID", EnvironmentVariableTarget.User);
                var Aws_Secret_Key =
                    Environment.GetEnvironmentVariable("LTD_USR_ACS_KEY", EnvironmentVariableTarget.User);

                var awsCredentials = new BasicAWSCredentials(Aws_Secret_Id, Aws_Secret_Key);
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
