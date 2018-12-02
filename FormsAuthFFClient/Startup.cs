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
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieSecure = CookieSecureOption.SameAsRequest
            });
        }
    }

    public class CustomMiddleware1
    {
        Func<IDictionary<string, object>, Task> _next;
        private readonly IAppBuilder _appBuilder;

        public CustomMiddleware1(Func<IDictionary<string, object>, Task> next, IAppBuilder appBuilder)
        {
            _next = next;
            _appBuilder = appBuilder;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            //No solution in place. Not really sure what to do....  
            //https://visualstudiomagazine.com/articles/2015/05/01/inject-custom-middleware.aspx
            new CookieAuthenticationMiddleware(, _appBuilder, new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieSecure = CookieSecureOption.SameAsRequest
            });
            //pre
            await _next.Invoke(environment);
            //post
        }
    }
}

