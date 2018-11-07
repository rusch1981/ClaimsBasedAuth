using System;
using System.Collections.Generic;

namespace IdetityServer.Models
{
    public partial class UsersRoles
    {
        public UsersRoles()
        {
            UsersClientsRoles = new HashSet<UsersClientsRoles>();
        }

        public int Id { get; set; }
        public string Role { get; set; }

        public ICollection<UsersClientsRoles> UsersClientsRoles { get; set; }
    }
}
