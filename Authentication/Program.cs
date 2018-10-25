using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace Authentication
{    
    class Program
    {
        static void Main(string[] args)
        {
            //Basic ClaimsIdentity example
            SetupClaimsPrincipal();
            CheckNewClaimsUsage();

            Console.WriteLine("--------------Break---------------");

            //Custom ClaimsIdentity Class example
            Thread.CurrentPrincipal = new ClaimsPrincipal( new CustomClaimsIdentity());
            CheckNewClaimsUsage();

            Console.WriteLine("--------------Break---------------");

            //Claims Transoformation from a WindowsPrinciple example
            SetCurrentPrincipal();
            CheckNewClaimsUsage();
            UseCurrentPrincipal();

            Console.ReadKey();
        }

        //This should be seperated out to another assembly in most usages
        //Basic authorization using attributes
        //This attribute has a custom override.  Refer to CustomAuthorizationManager.cs and
        //  the app.config.  It is magic!!!
        [ClaimsPrincipalPermission(SecurityAction.Demand, Operation = "Show", Resource = "Code")]
        private static void ShowMeTheCode()
        {
            Console.WriteLine("User Registered to ShowMeTheCode");
        }

        //This should be seperated out to another assembly in most usages
        //A more complex version of Authorization could involve passing collections of Resources and Operations(Actions)
        //  Refer to CustomAuthorisationManager.cs for more info you go this route you don't need to update
        //app.config like ShowMeTheCode().  It is not magic!!!
        private static void ShowMeThecode2()
        {
            ClaimsAuthorizationManager authManager =
                FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager;

            Collection<Claim> resourceClaims = new Collection<Claim>();
            resourceClaims.Add(new Claim("resource1", "Code"));
            resourceClaims.Add(new Claim("resource2", "Codes"));

            Collection<Claim> actionClaims = new Collection<Claim>();
            actionClaims.Add(new Claim("action1", "Show2"));
            actionClaims.Add(new Claim("action2", "update"));

            AuthorizationContext authContext = new AuthorizationContext(ClaimsPrincipal.Current, resourceClaims, actionClaims);
            if (authManager.CheckAccess(authContext))
            {
                Console.WriteLine("User Registered to ShowMeTheCode2");
            }
        }

        private static void UseCurrentPrincipal()
        {            
            ShowMeTheCode();
            ShowMeThecode2();            
        }

        private static void SetCurrentPrincipal()
        {
            //set current principal my current Windows User
            WindowsPrincipal incomingPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

            //This is how you set the transformed ClaimsPrinciple.  The class CustomClaimsTransformer is
            //  is being used in this example.  Refer to that class and to the app.config.  It is magic!!!
            Thread.CurrentPrincipal = FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                .ClaimsAuthenticationManager.Authenticate("none", incomingPrincipal);
        }

        private static void CheckNewClaimsUsage()
        {
            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            //WARNING IF YOU TRY TO RETRIEVE CLAIMS THAT DON'T EXIST YOU WILL GET A NULL RETURN TYPE.

            //retrieve current principal and print the Identity Authentication type and name to test functionality
            ClaimsPrincipal test1 = currentClaimsPrincipal;
            Console.WriteLine($"Is Authenticiated: {test1.Identities.FirstOrDefault().IsAuthenticated}");
            Console.WriteLine($"Identity Authentication TYPE: {test1.Identities.FirstOrDefault().AuthenticationType}");
            Console.WriteLine($"Name : {test1.Identities.FirstOrDefault().Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value}");
            Console.WriteLine($"Home Phone : {test1.Identities.FirstOrDefault().Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.HomePhone)).Value}");

            Console.WriteLine("EASIER WAYS Identity");
            ClaimsPrincipal test2 = currentClaimsPrincipal;

            //EASIER WAY TO CLAIMS IDENTITY - THERE SHOULD BE ONLY ONE            
            Console.WriteLine($"Is Authenticiated: {test2.Identity.IsAuthenticated}");            
            Console.WriteLine($"Identity Authentication TYPE: {test2.Identity.AuthenticationType}");

            Console.WriteLine("Even EASIER WAYS Identity and Claims");
            //EASIER WAY TO GET SPECIFIC CLAIMS and WITH A TESTs
            if (test2.HasClaim(x => x.Type == ClaimTypes.Name))
            {
                Console.WriteLine($"Identity Authentication TYPE: {test2.Identity.Name}");
                Console.WriteLine($"Name : {test2.FindFirst(ClaimTypes.Name).Value}");
                Console.WriteLine($"Home Phone : {test2.FindFirst(ClaimTypes.HomePhone).Value}");

            }
        }

        private static void SetupClaimsPrincipal()
        {
            IList<Claim> claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Andras")
                , new Claim(ClaimTypes.Country, "Sweden")
                , new Claim(ClaimTypes.Gender, "M")
                , new Claim(ClaimTypes.Surname, "Nemes")
                , new Claim(ClaimTypes.Email, "hello@me.com")
                , new Claim(ClaimTypes.Role, "IT")
            };

            //Create ClaimsIdentity and set authentication to "My Collection"
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claimCollection, "My Collection");

            //EASIER ANOTHER WAY TO ADD CLAIMS TO A CLAIMS IDENITY
            claimsIdentity.AddClaim(new Claim(ClaimTypes.HomePhone, "555-5555"));

            //We now test if this identity is authenticated or not:
            Console.WriteLine("Is Authenticiated: {claimsIdentity.IsAuthenticated}");

            //create and set Claims principal to current principle so it can be accessed from anywhere.
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            Thread.CurrentPrincipal = principal;
        }
    }
}
