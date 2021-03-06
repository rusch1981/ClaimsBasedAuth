﻿using System;
using System.Collections.Generic;

namespace IdentityServer.DAL.Models
{
    public partial class ClientSecrets
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }
        public int ClientId { get; set; }

        public Clients Client { get; set; }
    }
}
