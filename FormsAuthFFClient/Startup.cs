using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using IdentityServer3.Core;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(FormsAuthFFClient.Startup))]

namespace FormsAuthFFClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {           

            app.Use(typeof(CustomMiddleware2), app);

            //System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Sid;

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    CookieSecure = CookieSecureOption.SameAsRequest
            //});
        }
    }

    public class CustomMiddleware2 : OwinMiddleware
    {
        private readonly IAppBuilder _appBuilder;

        public CustomMiddleware2(OwinMiddleware next, IAppBuilder appBuilder) : base(next)
        {
            _appBuilder = appBuilder;
        }

        public async override Task Invoke(IOwinContext context)
        {
            var isRoute1 = context.Request.Uri.ToString().Contains("Home1");

            if (isRoute1)
            {
                var openIdConnectMiddleware = new OpenIdConnectAuthenticationMiddlewareWrapper(Next, _appBuilder, new OpenIdConnectAuthenticationOptions()
                {
                    Authority = "http://localhost:5000",

                    ClientId = "mvc2",
                    ClientSecret = "secret",
                    RedirectUri = "http://localhost:49820/",
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
                            var sub = id.FindFirst(IdentityServer3.Core.Constants.ClaimTypes.Subject);
                            var roles = id.FindFirst(IdentityServer3.Core.Constants.ClaimTypes.Role);
                            var name = id.FindFirst("name");


                            // create new identity and set name and role claim type
                            var nid = new ClaimsIdentity(
                                id.AuthenticationType,
                                IdentityServer3.Core.Constants.ClaimTypes.Name,
                                IdentityServer3.Core.Constants.ClaimTypes.Role);

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

                //Probably need to wrap the above in a class to make Next public
                var cookieAuthenticationMiddleware = new CookieAuthenticationMiddleware(openIdConnectMiddleware.NextWrapper, _appBuilder, new CookieAuthenticationOptions
                {
                    AuthenticationType = "Cookies"
                });



                AntiForgeryConfig.UniqueClaimTypeIdentifier = IdentityServer3.Core.Constants.ClaimTypes.Subject;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

                context.Response.Write("<h1>PreCustomMiddleware Meesage 1</h1>");
                await cookieAuthenticationMiddleware.Invoke(context);
                context.Response.Write("<h1>PostCustomMiddleware Meesage 1</h1>");
            }
            else
            {
                System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Sid;

                var authMiddleware2 = new CookieAuthenticationMiddleware(Next, _appBuilder, new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                    CookieSecure = CookieSecureOption.SameAsRequest
                });

                context.Response.Write("<h1>PreCustomMiddleware Meesage 2</h1>");
                await authMiddleware2.Invoke(context);
                context.Response.Write("<h1>PostCustomMiddleware Meesage 2</h1>");
            }
        }
    }

    public class OpenIdConnectAuthenticationMiddlewareWrapper : OpenIdConnectAuthenticationMiddleware
    {
        public OwinMiddleware NextWrapper
        {
            get
            {
                return Next;
            }
        }

        public OpenIdConnectAuthenticationMiddlewareWrapper(OwinMiddleware next, IAppBuilder appBuilder, OpenIdConnectAuthenticationOptions options) 
            : base(next, appBuilder, options)
        {

        }
    }

}