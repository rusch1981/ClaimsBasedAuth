using IdentityServer4.Test;
using System.Security.Claims;

namespace IdetityServer.UserStores
{
    public interface IUserStore
    {
        TestUser AutoProvisionUser(string provider, string userId, System.Collections.Generic.List<Claim> claims);
        TestUser FindByExternalProvider(string provider, string userId);
        TestUser FindBySubjectId(string subjectId);
        TestUser FindByUsername(string username);
        bool ValidateCredentials(string username, string password);
        bool HasClientAuthorization(string userName, string clientId);
    }
}