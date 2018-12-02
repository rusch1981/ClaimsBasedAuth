using FormsAuthFFClient.Models;
using FormsAuthFFClient.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace FormsAuthFFClient.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!User.Identity.IsAuthenticated)
                ViewBag.Message = "You Dont have enough Permissions, you need to be with elevated privileges to go there";
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                if (true) //Check the database
                {



                    List<Claim> claims = GetClaims(); //Get the claims from the headers or db or your user store
                    if (null != claims)
                    {
                        SignIn(claims);

                        return RedirectToLocal(returnUrl);
                    }



                    ModelState.AddModelError("", "Invalid username or password.");

                }
                else
                {
                    //No User of that email address
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            else
            {
                //Model not valid
                ModelState.AddModelError("", "The Model is Not valid");
            }
            // If we got this far, something failed, redisplay form
            return View(model);

        }

        private List<Claim> GetClaims()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(DemoIdentity.IdClaimType, "12345"));
            claims.Add(new Claim(ClaimTypes.Sid, "6789"));
            claims.Add(new Claim(ClaimTypes.Name, "Adam"));
            claims.Add(new Claim(ClaimTypes.Name, "Rusch"));

            var roles = new[] { "Admin", "Citizin", "Worker" };
            var groups = new[] { "Admin", "Citizin", "Worker" };

            foreach (var item in roles)
            {
                claims.Add(new Claim(DemoIdentity.RolesClaimType, item));
            }
            foreach (var item in groups)
            {
                claims.Add(new Claim(DemoIdentity.GroupClaimType, item));
            }
            return claims;
        }


        private void SignIn(List<Claim> claims)//Mind!!! This is System.Security.Claims not WIF claims
        {

            var claimsIdentity = new DemoIdentity(claims,
            DefaultAuthenticationTypes.ApplicationCookie);

            //This uses OWIN authentication
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, claimsIdentity);


            HttpContext.User = new DemoPrincipal(AuthenticationManager.AuthenticationResponseGrant.Principal);



        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}