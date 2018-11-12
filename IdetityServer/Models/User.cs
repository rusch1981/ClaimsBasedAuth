using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.Models
{
    public class User
    {
        //
        // Summary:
        //     Gets or sets the subject identifier.
        public string SubjectId { get; set; }
        //
        // Summary:
        //     Gets or sets the username.
        public string Username { get; set; }
        //
        // Summary:
        //     Gets or sets the claims.
        public ICollection<Claim> Claims { get; set; }
    }
}
