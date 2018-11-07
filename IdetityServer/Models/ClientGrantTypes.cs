using System;
using System.Collections.Generic;

namespace IdetityServer.Models
{
    public partial class ClientGrantTypes
    {
        public int Id { get; set; }
        public string GrantType { get; set; }
        public int ClientId { get; set; }

        public Clients Client { get; set; }
    }
}
