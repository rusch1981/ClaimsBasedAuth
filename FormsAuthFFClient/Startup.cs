using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(FormsAuthFFClient.Startup))]

namespace FormsAuthFFClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Sid;

            app.Use(typeof(CustomMiddleware1), app);

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    CookieSecure = CookieSecureOption.SameAsRequest
            //});
        }
    }

    public class CustomMiddleware1 : OwinMiddleware
    {
        private readonly IAppBuilder _appBuilder;

        public CustomMiddleware1(OwinMiddleware next, IAppBuilder appBuilder) : base(next)
        {
            _appBuilder = appBuilder;
        }

        public async override Task Invoke(IOwinContext context)
        {
            if(context.Request.Uri.ToString().Contains("49820"))
            {
                var authMiddleware1 = new CookieAuthenticationMiddleware(Next, _appBuilder, new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                    CookieSecure = CookieSecureOption.SameAsRequest
                });

                context.Response.Write("<h1>PreCustomMiddleware1</h1>");
                await authMiddleware1.Invoke(context);
                context.Response.Write("<h1>PostCustomMiddleware1</h1>");
            }
            
        }
    }
}