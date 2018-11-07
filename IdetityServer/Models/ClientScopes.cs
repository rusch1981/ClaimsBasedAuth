using System;
using System.Collections.Generic;

namespace IdetityServer.Models
{
    public partial class ClientScopes
    {
        public int Id { get; set; }
        public string Scope { get; set; }
        public int ClientId { get; set; }

        public Clients Client { get; set; }
    }
}
