using IdentityServer4.Models;
using IdentityServer4.Services;
using IdetityServer.Models;
using IdetityServer.UserStores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdetityServer.ProfileServices
{
    public class CustomProfileService : IProfileService
    {
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger Logger;
        private IUserStore UserStore;
        private IdentityServerContext _DataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProfileService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CustomProfileService(ILogger<DefaultProfileService> logger, IUserStore userStore)
        {
            Logger = logger;
            UserStore = userStore;
            _DataContext = new IdentityServerContext();
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(Logger);
            //  do not remove.  This is need to populate all tokens with the subject claim
            context.AddRequestedClaims(context.Subject.Claims);

            var aD_User = UserStore.FindBySubjectId(context.Subject.Claims.FirstOrDefault(x => x.Type == "sub").Value);
            int dB_UserId = _DataContext.Users.FirstOrDefault(x => x.UserName == aD_User.Username).Id;
            int dB_ClientId = _DataContext.Clients.FirstOrDefault(x => x.ClientId == context.Client.ClientId).Id;            
            List<Claim> issuedClaims = new List<Claim>();     
            StringBuilder allRoles = new StringBuilder("");


            foreach (var userClientRoles in _DataContext.UsersClientsRoles.Where(x => x.UserId == dB_UserId && x.ClientId == dB_ClientId && x.UserRoleId != 0))
            {
                string role = _DataContext.UsersRoles.FirstOrDefault(x => x.Id == userClientRoles.UserRoleId).Role;

                allRoles.Append(" " + role);
            }

            List<Claim> allClaims = new List<Claim>
            {
                new Claim("name", aD_User.Username),
                new Claim("role", allRoles.ToString())
            };

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

            //add claims to context
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
    }
}
