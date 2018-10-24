using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Authentication
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupClaimsPrincipal();
            CheckNewClaimsUsage();

            CustomClaimsIdentity customeIdentity = new CustomClaimsIdentity();
            CheckNewClaimsUsage(new ClaimsPrincipal(customeIdentity));

            Console.ReadKey();
        }

        private static void CheckNewClaimsUsage(ClaimsPrincipal claimsPrincipal = null)
        {
            ClaimsPrincipal currentClaimsPrincipal;

            if (claimsPrincipal == null)
            {
                currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            }
            else
            {
                currentClaimsPrincipal = claimsPrincipal;
            }

            //retrieve current principal and print the Identity Authentication type and nae name to test functionality
            ClaimsPrincipal test = currentClaimsPrincipal;
            Console.WriteLine($"Identity Authentication TYPE: {test.Identities.FirstOrDefault().AuthenticationType}");


            Console.WriteLine($"Name : {test.Identities.FirstOrDefault().Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value}");


            Console.WriteLine("EASIER WAYS");
            //EASIER WAY TO GET CURRENT PRINCIPLE
            ClaimsPrincipal test2 = currentClaimsPrincipal;

            //We now test if this identity is authenticated or not:
            Console.WriteLine("Is Authenticiated: {claimsIdentity.IsAuthenticated}");

            //EASIER WAY TO CLAIMS IDENTITY - THERE SHOULD BE ONLY ONE
            Console.WriteLine($"Identity Authentication TYPE: {test2.Identity.AuthenticationType}");

            //EASIER WAY TO GET SPECIFIC CLAIMS and WITH A TEST
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
