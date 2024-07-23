#define SEGURIDAD

using Owin;
using Principal.Server.Guards;
using System.Collections.Generic;
using System.Linq;
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
#if SEGURIDAD
            appBuilder.Use(async (context, next) =>
            {
                var encryptedClaims = context.Request.Headers.Get("Custom-Claims");
                if (!string.IsNullOrWhiteSpace(encryptedClaims))
                {
                    var claims = DecodeCustomClaims(encryptedClaims);

                    var identity = new ClaimsIdentity(claims, "CustomAuthType");
                    context.Request.User = new ClaimsPrincipal(identity);
                }

                await next.Invoke();
            });
#endif
            appBuilder.UseWebApi(config);
        }

#if SEGURIDAD
        private static List<Claim> DecodeCustomClaims(string encryptedClaims)
        {
            var claims = new List<Claim>();
            if (RequestValidator.ValidSign(encryptedClaims, out var resultantClaims))
            {
                claims.AddRange(resultantClaims.Select(claim => new Claim(claim.Type, claim.Value)));
            }

            return claims;
        }
#endif
    }
}
