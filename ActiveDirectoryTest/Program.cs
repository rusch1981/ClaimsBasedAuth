using System;
using System.DirectoryServices.AccountManagement;  // required for PrincipalContext,  PrincipalSearcher and UserPrincipal
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace ActiveDirectoryTest
{
    class Program
    {
        static void Main(string[] args)
        {

            bool validUser = false;
            //username and password must be valid for valid User Validation to return true
            string username = "arusch";
            string password = "";


            Console.WriteLine("Begin User Validation.............................");
            //Encapsulates the server or domain against which all operations are performed. 
            using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain))
            {
                //validates user based on username and password
                validUser = domainContext.ValidateCredentials(username, password);
            }

            Console.WriteLine($"User is valid: {validUser}");
            Console.WriteLine("End User Validation.............................");
            Console.WriteLine();

            Console.WriteLine("Begin User Retrieval.............................");

            //Encapsulates the server or domain against which all operations are performed. 
            using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain))
            {
                //validates user based on username and password
                validUser = domainContext.ValidateCredentials(username, password);

                if (validUser)
                {
                    //Create a "user object" in the context
                    UserPrincipal user = new UserPrincipal(domainContext);

                    //Specify the search parameters
                    user.SamAccountName = username;

                    //Create the searcher
                    PrincipalSearcher pS = new PrincipalSearcher();
                    pS.QueryFilter = user;

                    //Perform the search
                    PrincipalSearchResult<Principal> results = pS.FindAll();

                    foreach (var result in results)
                    {
                        Console.WriteLine("User Sid: " + result.Sid);
                        Console.WriteLine("User Name: " + result.Name);
                        Console.WriteLine("User SamAccountName: " + result.SamAccountName);
                        Console.WriteLine("User DisplayName: " + result.DisplayName);
                        Console.WriteLine("User DistinguishedName: " + result.DistinguishedName);
                        Console.WriteLine("User UserPrincipalName: " + result.UserPrincipalName);
                    }
                }
            }

            Console.WriteLine("End User Retrieval.............................");

            Console.WriteLine("Begin CurrentUser Retrieval.............................");

            //retrieve current prinicipal  
            WindowsPrincipal incomingPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

            // set cast current principal to claims principal
            ClaimsPrincipal test1 = incomingPrincipal as ClaimsPrincipal;

            Console.WriteLine($"Is Authenticiated: {test1.Identities.FirstOrDefault().IsAuthenticated}");
            Console.WriteLine($"Identity Authentication TYPE: {test1.Identities.FirstOrDefault().AuthenticationType}");
            Console.WriteLine($"Name : {test1.Identities.FirstOrDefault().Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value}");


            //set Claimsprincipal to current prinicipal
            Thread.CurrentPrincipal = test1;

            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            Console.WriteLine($"Is Authenticiated: {currentClaimsPrincipal.Identities.FirstOrDefault().IsAuthenticated}");
            Console.WriteLine($"Identity Authentication TYPE: {currentClaimsPrincipal.Identities.FirstOrDefault().AuthenticationType}");
            Console.WriteLine($"Name : {currentClaimsPrincipal.Identities.FirstOrDefault().Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value}");

            Console.WriteLine("End CurrentUser Retrieval.............................");
            Console.WriteLine();

            Console.WriteLine("Begin All User Retrieval.............................");


            //Encapsulates the server or domain against which all operations are performed. 
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                //  Create Principal Searcher for this context
                //  UserPrincipal with no param other than context returns all users on this domain.
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        Console.WriteLine("User Sid: " + result.Sid);
                        Console.WriteLine("User Name: " + result.Name);
                        Console.WriteLine("User SamAccountName: " + result.SamAccountName);
                        Console.WriteLine("User DisplayName: " + result.DisplayName);
                        Console.WriteLine("User DistinguishedName: " + result.DistinguishedName);
                        Console.WriteLine("User UserPrincipalName: " + result.UserPrincipalName);

                        //turned off because it takes way to long query.  
                        //foreach(var group in result.GetGroups())
                        //{
                        //    Console.WriteLine($"{result.DisplayName} is member of the {group.Name} group");
                        //}

                    }
                }
            }

            Console.WriteLine("End All User Retrieval.............................");

            Console.WriteLine();
        }
    }
}
