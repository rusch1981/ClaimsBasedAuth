using System;
using System.Collections.Generic;

namespace IdentityServer.Models
{
    public partial class Users
    {
        public Users()
        {
            UsersClientsRoles = new HashSet<UsersClientsRoles>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }

        public ICollection<UsersClientsRoles> UsersClientsRoles { get; set; }
    }
}
