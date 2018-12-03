using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace FormsAuthFFClient.Filters
{
    public class CustomAuthAttribute : AuthorizeAttribute
    {
        public string AuthArea { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var uri = HttpContext.Current.Request.Url.ToString().ToLower();
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var areaClaim = identity.Claims.FirstOrDefault(x => x.Type == "Area");

            if(areaClaim == null)
            {
                return false;
            }

            switch (areaClaim.Value)
            {
                case "General":
                    if (uri.Contains("about"))
                    {
                        return base.AuthorizeCore(httpContext);
                    }
                    else
                    {
                        AuthenticationManager.SignOut();
                    }
                    break;
                case "Area1":
                    if(uri.Contains("area1"))
                    {
                        return base.AuthorizeCore(httpContext);
                    }
                    else
                    {
                        AuthenticationManager.SignOut();
                    }
                    break;
                case "Area2":
                    if (uri.Contains("area2"))
                    {
                        return base.AuthorizeCore(httpContext);
                    }
                    else
                    {
                        AuthenticationManager.SignOut();
                    }
                    break;
                default:
                    AuthenticationManager.SignOut();
                    return false;
            }
            //Will never happen
            return false;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
    }
}