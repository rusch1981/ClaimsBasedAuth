﻿using IdentityModel;
using IdentityServer4.Test;
using IdentityServer.Models;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using IdentityServer.Models.Account;

namespace IdentityServer.UserStores
{
    public class ActiveDirectoryUserStore : IUserStore
    {
        #region Constants, Enums
        #endregion
        #region Fields

        private readonly List<User> _users;
        private readonly DAL.Models.IdentityServerContext _dataContext;
        private readonly StringBuilder _errorMessage;

        #endregion
        #region Properties	        

        public string ErrorMessage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_errorMessage.ToString()))
                {
                    return AccountOptions.GenericLogginError;
                }

                return _errorMessage.ToString();
            }
            private set
            {
                _errorMessage.Clear();
                _errorMessage.Append(value);
            }
        }

        #endregion
        #region Constructors/Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestUserStore"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        public ActiveDirectoryUserStore(DAL.Models.IdentityServerContext dataContext)
        {
            _users = new List<User>();
            _dataContext = dataContext;
            _errorMessage = new StringBuilder();
        }

        #endregion
        #region Events/Methods	

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool IsValidUser(string username, string password)
        {
            //Encapsulates the server or domain against which all operations are performed. 
            using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain))
            {


                if (!_dataContext.Users.Any(x => x.UserName == username))
                {
                    ErrorMessage = AccountOptions.InvalidUserErrorMessage;
                    return false;
                }

                // check to see if the user is enabled and the credentials are valid.  
                if (!domainContext.ValidateCredentials(username, password) ||
                    UserPrincipal.FindByIdentity(domainContext, username)?.Enabled == false)
                {
                    ErrorMessage = AccountOptions.InvalidCredentialsErrorMessage;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Finds the user by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public User FindBySubjectId(string subjectId)
        {
            using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain))
            {
                using (UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, IdentityType.Sid, (subjectId)))
                {
                    User newUser = new User();
                    newUser.SubjectId = user.Sid.ToString();
                    newUser.Username = user.SamAccountName;
                    newUser.Claims = new List<Claim> { new Claim(JwtClaimTypes.Name, user.DisplayName) };

                    return newUser;
                }
            }
        }

        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public User FindByUsername(string username)
        {
            using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain))
            {
                //Create a "user object" in the context
                using (UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, username))
                {
                    User newUser = new User();
                    newUser.SubjectId = user.Sid.ToString();
                    newUser.Username = user.SamAccountName;
                    newUser.Claims = new List<Claim>{ new Claim(JwtClaimTypes.Name, user.DisplayName) };

                    return newUser;
                }
            }
        }

        /// <summary>
        /// Finds the user by external provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public User FindByExternalProvider(string provider, string userId)
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
        public User AutoProvisionUser(string provider, string userId, List<Claim> claims)
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
            var user = new User
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
        #endregion
    }
}