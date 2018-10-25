using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace WebApplicationAuthentication
{
    //You will need to references to System.IdentityModel and System.IdentityModel.Services
    //This is very Similar to the CustomClaimsTransformer found in the Authentication Project
    //  in this solution.
    public class CustomClaimsTransformer : ClaimsAuthenticationManager
    {
        //Go to commented out section of the global.asax (Application_PostAuthenticateRequest) to see how the auto magic happens.  We are using caching 
        // so we don't call this method from Application_PostAuthenticateRequest any longer.  
        //For cahcing go to :  AccountController POST Login() method
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            //If the user is anonymous then we let the base class handle the call. The Authenticate method 
            //  in the base class will simply return the incoming principal so an unauthenticated user will 
            //  not gain access to any further claims and will stay anonymous.
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return base.Authenticate(resourceName, incomingPrincipal);
            }

            //do something if authenticated like query a database for more claims.
            ClaimsPrincipal transformedPrincipal = DressUpPrincipal(incomingPrincipal.Identity.Name);

            //Cache the session - See Web.config to for configuration
            CreateSession(transformedPrincipal);

            return transformedPrincipal;
        }


        //We create a SessionSecurityToken object and pass in the transformed principal and an expiration. 
        //  By default the auth session mechanism works with absolute expiration, we’ll see later how to implement sliding 
        //  expiration. Then we write that token to a cookie. From this point on we don’t need to run the transformation 
        //  logic any longer.
        private void CreateSession(ClaimsPrincipal transformedPrincipal)
        {
            SessionSecurityToken sessionSecurityToken = new SessionSecurityToken(transformedPrincipal, TimeSpan.FromHours(8));
            FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(sessionSecurityToken);
        }

        //You will typically not have access to a lot of claims right after authentication. You can count
        // with the bare minimum of the UserName and often that’s all you get in a forms-based login 
        //  scenario. So we will simulate a database lookup based on the user name. The user’s claims are 
        //  probably stored somewhere in some storage.
        private ClaimsPrincipal DressUpPrincipal(string userName)
        {
            List<Claim> claims = new List<Claim>();

            //simulate database lookup - The userName is from an account you registered.
            if (userName.IndexOf("rusch1981@hotmail.com", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                claims.Add(new Claim(ClaimTypes.Country, "United State"));
                claims.Add(new Claim(ClaimTypes.GivenName, "Adam"));
                claims.Add(new Claim(ClaimTypes.Name, "Adam"));

                //This is needed to get around the InvalidOperationException
                claims.Add(new Claim(ClaimTypes.NameIdentifier, "Adam"));
                claims.Add(new Claim(ClaimTypes.Role, "dev"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.GivenName, userName));
                claims.Add(new Claim(ClaimTypes.Name, userName));

                //This is needed to get around the InvalidOperationException
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userName));
            }

            //Create ClaimsPrinicipal and set the Idenity AuthroizationType to "Custom" which is arbitrary and just an 
            //  example AuthorizationType.  
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));
        }
    }
}