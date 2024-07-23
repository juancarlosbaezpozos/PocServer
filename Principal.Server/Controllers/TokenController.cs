using Principal.Server.Guards;
using System.Collections.Generic;
using System.Web.Http;

namespace Principal.Server.Controllers
{
    [Authorize]
    public class TokenController : ApiController
    {
        // GET api/values 
        public IHttpActionResult Get()
        {
            RequestValidator.ValidateAppRole("access_as_application");

            return Ok(new string[] { "valor1", "valor2" });
        }

    }
}
