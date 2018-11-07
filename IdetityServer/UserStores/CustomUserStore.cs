//using IdenityServer4Example.Models;
using IdentityModel;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace IdetityServer.UserStores
{
    public class CustomUserStore : IUserStore
    {
        private List<TestUser> _users;
        private bool _isValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestUserStore"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        public CustomUserStore()
        {
            _users = new List<TestUser>();
        }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool ValidateCredentials(string username, string password)
        {
            //Encapsulates the server or domain against which all operations are performed. 
            using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain))
            {
                //validates user based on username and password
                _isValid = domainContext.ValidateCredentials(username, password);

                if (_isValid)
                {
                    //Create a "user object" in the context
                    using (UserPrincipal user = new UserPrincipal(domainContext))
                    {
                        //Specify the search parameters
                        user.SamAccountName = username;

                        //Create the searcher
                        PrincipalSearcher searcher = new PrincipalSearcher();
                        //Define Filter
                        searcher.QueryFilter = user;

                        //Query Results based on filter
                        PrincipalSearchResult<System.DirectoryServices.AccountManagement.Principal> results = searcher.FindAll();

                        if (results.Count() == 1)
                        {
                            _users = new List<TestUser>
                            {
                                new TestUser
                                {
                                    SubjectId = results.FirstOrDefault().Sid.ToString(),
                                    Username = results.FirstOrDefault().SamAccountName,
                                    Claims =
                                    {
                                        new Claim(JwtClaimTypes.Name, results.FirstOrDefault().DisplayName)
                                    }
                                }
                            };
                        }
                        else
                        {
                            // Probably should throw an exception or notify admin if multiple users returned but for now just fail the login
                            _isValid = false;
                        }
                    }
                }
            }

            return _isValid;
        }

        /// <summary>
        /// Finds the user by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public TestUser FindBySubjectId(string subjectId)
        {
            if (_isValid && _users.Any(x => x.SubjectId == subjectId))
            {
                return _users.FirstOrDefault(x => x.SubjectId == subjectId);
            }

            return LookUpSubjectById(subjectId);
        }

        /// <summary>
        /// Looks up User by subject identifier from Active Directory.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        private TestUser LookUpSubjectById(string subjectId)
        {
            using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain))
            {
                //Create a "user object" in the context
                using (UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, IdentityType.Sid, (subjectId)))
                {

                    if (user != null)
                    {
                        _users = new List<TestUser>
                            {
                                new TestUser
                                {
                                    SubjectId = user.Sid.ToString(),
                                    Username = user.SamAccountName,
                                    Claims =
                                    {
                                        new Claim(JwtClaimTypes.Name, user.DisplayName)
                                    }
                                }
                            };                        
                    }
                    else
                    {                        
                        // Probably should throw an exception if zero users returned but for now just fail the login
                        _users = new List<TestUser>();
                    }

                    return _users.FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public TestUser FindByUsername(string username)
        {
            if (_isValid && _users.Any(x => x.Username == username))
            {
                return _users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }

            return LookUpSubjectByName(username);
        }

        /// <summary>
        /// Looks up User by username from Active Directory.
        /// </summary>
        /// <param name="username">The subject identifier.</param>
        /// <returns></returns>
        private TestUser LookUpSubjectByName(string name)
        {
            using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain))
            {
                //Create a "user object" in the context
                using (UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, name))
                {
                    if (user != null)
                    {
                        _users = new List<TestUser>
                            {
                                new TestUser
                                {
                                    SubjectId = user.Sid.ToString(),
                                    Username = user.SamAccountName,
                                    Claims =
                                    {
                                        new Claim(JwtClaimTypes.Name, user.DisplayName)
                                    }
                                }
                            };
                    }
                    else
                    {
                        // Probably should throw an exception if zero users returned but for now just fail the login
                        _users = new List<TestUser>();
                    }

                    return _users.FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Finds the user by external provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public TestUser FindByExternalProvider(string provider, string userId)
        {
            return _users.FirstOrDefault(x =>
                x.ProviderName == provider &&
                x.ProviderSubjectId == userId);
        }

        /// <summary>
        /// Automatically provisions a user.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        public TestUser AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            foreach (var claim in claims)
            {
                // if the external system sends a display name - translate that to the standard OIDC name claim
                if (claim.Type == ClaimTypes.Name)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
                }
                // if the JWT handler has an outbound mapping to an OIDC claim use that
                else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type))
                {
                    filtered.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
                }
                // copy the claim as-is
                else
                {
                    filtered.Add(claim);
                }
            }

            // if no display name was provided, try to construct by first and/or last name
            if (!filtered.Any(x => x.Type == JwtClaimTypes.Name))
            {
                var first = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value;
                var last = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value;
                if (first != null && last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
                }
                else if (first != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                }
                else if (last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            // create a new unique subject id
            var sub = CryptoRandom.CreateUniqueId();

            // check if a display name is available, otherwise fallback to subject id
            var name = filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? sub;

            // create new user
            var user = new TestUser
            {
                SubjectId = sub,
                Username = name,
                ProviderName = provider,
                ProviderSubjectId = userId,
                Claims = filtered
            };

            // add user to in-memory store
            _users.Add(user);

            return user;
        }

        /// <summary>
        /// Tests access rights for the user against client.  
        /// </summary>
        /// <param name="userName">The user identifier.</param>
        /// <param name="clientId">The claims.</param>
        /// <returns></returns>
        public bool HasClientAuthorization(string userName, string clientId)
        {
            //test against database to see if UserName has authrization rights
            //IdentityServer4Context context = new IdentityServer4Context();
            //Users user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            //Clients client = context.Clients.Where(c => c.ClientId == clientId).FirstOrDefault();

            //if (user != null && client != null)
            //{
            //    return context.UsersScopes.Where(u => u.ClientsId == client.Id && u.UsersId == user.Id).Any();
            //}

            return false;
        }
    }
}