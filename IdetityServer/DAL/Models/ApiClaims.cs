using System;
using System.Collections.Generic;

namespace IdentityServer.DAL.Models
{
    public partial class ApiClaims
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiResourceId { get; set; }

        public ApiResources ApiResource { get; set; }
    }
}
