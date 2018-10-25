using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Claims;

namespace Authentication
{
    //ClaimsAuthenticationManager is from the System.Security.Claims namespace but you MUST add a
    //  reference to the System.IdentityModel.dll or it will not be found.  
    //This process is not neccessary and .Net4.5 has built in the ability to tranform Prinicipals.  This
    //  example is merely showing that you can and should create ClaimsPrincipals with only the data that is 
    //  required.  
    //In order for this work you need to register your custom claims Manager (Transformer) in app.config. 
    //  Before you do that add a reference to the System.identitymodel.services.dll. See app.config.  
    //THIS CLASS IS NEVER REFERENCED DIRECTLY
    public class CustomClaimsTransformer : ClaimsAuthenticationManager
    {
        //Overrid the Authenticate method
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            //validate name claim - This should be custom logic.  This is just an example
            string nameClaimValue = incomingPrincipal.Identity.Name;

            if (string.IsNullOrEmpty(nameClaimValue))
            {
                throw new SecurityException("A user with no name???");
            }

            return CreatePrincipal(nameClaimValue);
        }

        //  This is where the transformation occurs.  
        private ClaimsPrincipal CreatePrincipal(string userName)
        {
            // The code up to the return is simulating a database call and is arbitrary.  
            bool isRegisteredUser = false;

            //if you want your ClaimsPrincipal "Registered" claim to be true you will need to 
            //  provide the correct AD user name for your current user.
            if (userName.IndexOf("DESKTOP-PROEALE\\rusch", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                isRegisteredUser = true;
            }

            List<Claim> claimsCollection = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim("Registered", isRegisteredUser.ToString()),
                new Claim(ClaimTypes.HomePhone, "555-7777")
            };

            //Set the ClaimsIdentity AuthorizationType to "Custom."  "custom" is arbitrary 
            //it can be anything you like.
            return new ClaimsPrincipal(new ClaimsIdentity(claimsCollection, "Custom"));
        }
    }
}
