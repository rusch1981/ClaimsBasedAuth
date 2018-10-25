using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplicationAuthentication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //how in the hell does Application_PostAuthenticateRequest get called?  
        //There is a comprehensive article on that by Rick Strahl.In short, the runtime uses reflection on your global application class.
        //http://weblog.west-wind.com/posts/2009/Jun/18/How-do-ASPNET-Application-Events-Work
        //This type of event wiring is usually called "automatic" and is also present at page level. For example, the Page_Load is called just because of the default auto wireup.
        //http://msdn.microsoft.com/en-us/library/system.web.configuration.pagessection.autoeventwireup(v=vs.110).aspx

        //Our custom Authenticate method in CustomClaimsTransformer.cs runs with every single page request. Set a breakpoint within 
        //  the method and code execution will stop every time you navigate to a page in the web app. The real implementation of the 
        //  Authenticate method will most likely involve some expensive DB lookup which then also runs with every page request. That’s 
        //  of course not maintainable.  How can we solve that problem? By caching the outcome of the Authenticate method in a built-in 
        //  authentication session.
        //Removed to utilize Caching  via the AccountController POST Login() method
        //protected void Application_PostAuthenticateRequest()
        //{
        //    ClaimsPrincipal currentPrincipal = ClaimsPrincipal.Current;
        //    CustomClaimsTransformer customClaimsTransformer = new CustomClaimsTransformer();
        //    ClaimsPrincipal tranformedClaimsPrincipal = customClaimsTransformer.Authenticate(string.Empty, currentPrincipal);

        //    //This is needed to get around the InvalidOperationException
        //    AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;


        //    Thread.CurrentPrincipal = tranformedClaimsPrincipal;
        //    HttpContext.Current.User = tranformedClaimsPrincipal;
        //}
    }
}
