using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.UserStores
{
    public interface IUserStore
    {
        string ErrorMessage { get; }
        User AutoProvisionUser(string provider, string userId, System.Collections.Generic.List<Claim> claims);
        User FindByExternalProvider(string provider, string userId);
        User FindBySubjectId(string subjectId);
        User FindByUsername(string username);
        bool IsValidUser(string username, string password);
    }
}