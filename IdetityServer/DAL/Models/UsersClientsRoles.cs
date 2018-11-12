using System;
using System.Collections.Generic;

namespace IdentityServer.DAL.Models
{
    public partial class UsersClientsRoles
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public int UserRoleId { get; set; }

        public Clients Client { get; set; }
        public Users User { get; set; }
        public UsersRoles UserRole { get; set; }
    }
}
