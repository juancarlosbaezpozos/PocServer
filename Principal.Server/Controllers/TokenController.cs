using System.Collections.Generic;
using System.Web.Http;

namespace Principal.Server.Controllers
{
    public class TokenController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "valor1", "valor2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "valorid";
        }

        // POST api/values 
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
