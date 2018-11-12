using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServer.Models;
using IdentityServer.UserStores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.ProfileServices
{
    public class ActiveDirectoryProfileService : IProfileService
    {
        #region Constants, Enums
        #endregion
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger Logger;
        private readonly IUserStore UserStore;
        private readonly IdentityServerContext _DataContext;

        #endregion
        #region Properties
        #endregion
        #region Constructors/Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProfileService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ActiveDirectoryProfileService(ILogger<DefaultProfileService> logger, IUserStore userStore, IdentityServerContext dataContext)
        {
            Logger = logger;
            UserStore = userStore;
            _DataContext = dataContext;
        }

        #endregion
        #region Events/Methods		

        private string RetrieveUserRolesBasedOnClientContext(Client client, string userName)
        {
            StringBuilder allRoles = new StringBuilder(string.Empty);
            int? dB_UserId = GetDababaseUserId(userName);
            int? dB_ClientId = GetDababaseClientId(client);

            if (dB_ClientId == null || dB_UserId == null)
            {
                return string.Empty;
            }

            foreach (var userClientRoles in _DataContext.UsersClientsRoles.Where(x => x.UserId == dB_UserId && x.ClientId == dB_ClientId && x.UserRoleId != 0))
            {
                string role = _DataContext.UsersRoles.FirstOrDefault(x => x.Id == userClientRoles.UserRoleId).Role;
                if (allRoles.ToString().Equals(string.Empty))
                {
                    allRoles.Append(role);
                }
                else
                {
                    allRoles.Append(" " + role);
                }
            }

            return allRoles.ToString();
        }

        /// <summary>
        /// Get IdentityServer Database User ID.
        /// If no user exists will return null
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private int? GetDababaseUserId(string userName)
        {
            if (_DataContext.Users.FirstOrDefault(x => x.UserName == userName) != null)
            {
                return _DataContext.Users.FirstOrDefault(x => x.UserName == userName).Id;
            }

            return null;
        }

        /// <summary>
        /// Get IdentityServer Database Client ID
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private int? GetDababaseClientId(Client client)
        {
            if (_DataContext.Clients.FirstOrDefault(x => x.ClientId == client.ClientId) != null)
            {
                return _DataContext.Clients.FirstOrDefault(x => x.ClientId == client.ClientId).Id;
            }

            return null;
        }

        /// <summary>
        /// Get User from Active Directory
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        private TestUser GetActiveDirectoryUser(ClaimsPrincipal principal)
        {
            return UserStore.FindBySubjectId(principal.Claims.FirstOrDefault(x => x.Type == "sub").Value);
        }
        
        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(Logger);

            string userName = GetActiveDirectoryUser(context.Subject).Username;            

            List<Claim> allClaims = new List<Claim>
            {
                new Claim("name", userName),
                new Claim("role", RetrieveUserRolesBasedOnClientContext(context.Client, userName))
            };

            List<Claim> issuedClaims = new List<Claim>();

            //Only issue requested claims
            if (context.RequestedClaimTypes.Any())
            {
                foreach (var claim in allClaims)
                {
                    if (context.RequestedClaimTypes.Any(x => x.Equals(claim.Type, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        issuedClaims.Add(claim);
                    }
                }
            }

            //  do not remove.  This is need to populate all tokens with the subject claim
            context.AddRequestedClaims(context.Subject.Claims);
            context.IssuedClaims = issuedClaims;

            context.LogIssuedClaims(Logger);

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual Task IsActiveAsync(IsActiveContext context)
        {
            Logger.LogDebug("IsActive called from: {caller}", context.Caller);

            context.IsActive = true;
            return Task.CompletedTask;
        }

        #endregion
    }
}
