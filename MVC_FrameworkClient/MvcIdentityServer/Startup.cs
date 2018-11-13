using IdentityServer3.Core;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace MvcIdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "http://localhost:5000",

                ClientId = "mvc2",
                ClientSecret = "secret",
                RedirectUri = "https://localhost:44330",
                ResponseType = "code id_token",

                Scope = "openid profile customProfile1 customProfile2",
                //this is avoid error with https redirect
                RequireHttpsMetadata = false,
                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,


                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        // we want to keep first name, last name, subject and roles
                        var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                        var roles = id.FindFirst(Constants.ClaimTypes.Role);
                        var name = id.FindFirst("name");


                        // create new identity and set name and role claim type
                        var nid = new ClaimsIdentity(
                            id.AuthenticationType,
                            Constants.ClaimTypes.Name,
                            Constants.ClaimTypes.Role);

                        nid.AddClaim(sub);
                        nid.AddClaim(roles);
                        nid.AddClaim(name);

                        n.AuthenticationTicket = new AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    }
                }

            });
        }
    }
}