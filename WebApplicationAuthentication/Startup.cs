using Microsoft.Owin;
using Owin;
using System.Security.Claims;
using System.Threading;
using System.Web;

[assembly: OwinStartupAttribute(typeof(WebApplicationAuthentication.Startup))]
namespace WebApplicationAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }        
    }
}
