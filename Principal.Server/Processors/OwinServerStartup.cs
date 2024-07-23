using Owin;
using Principal.Server.Guards;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;

namespace Principal.Server.Processors
{
    public class OwinServerStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.Use(async (context, next) =>
            {
                string encryptedClaims = context.Request.Headers.Get("Custom-Claims");
                if (!string.IsNullOrWhiteSpace(encryptedClaims))
                {
                    List<Claim> claims = DecodeCustomClaims(encryptedClaims);

                    var identity = new ClaimsIdentity(claims, "CustomAuthType");
                    context.Request.User = new ClaimsPrincipal(identity);
                }

                await next.Invoke();
            });

            appBuilder.UseWebApi(config);
        }

        private List<Claim> DecodeCustomClaims(string encryptedClaims)
        {
            List<Claim> claims = new List<Claim>();
            if (RequestValidator.ValidSign(encryptedClaims, out var resultantClaims))
            {
                foreach (var claim in resultantClaims)
                {
                    claims.Add(new Claim(claim.Type, claim.Value));
                }
            }

            return claims;
        }
    }
}
