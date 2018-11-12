using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer.UserStores
{
    public interface IUserStore
    {
        string ErrorMessage { get; }
        TestUser AutoProvisionUser(string provider, string userId, System.Collections.Generic.List<Claim> claims);
        TestUser FindByExternalProvider(string provider, string userId);
        TestUser FindBySubjectId(string subjectId);
        TestUser FindByUsername(string username);
        bool IsValidUser(string username, string password);
    }
}