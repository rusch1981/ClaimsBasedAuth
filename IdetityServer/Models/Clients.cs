using System;
using System.Collections.Generic;

namespace IdentityServer.Models
{
    public partial class Clients
    {
        public Clients()
        {
            ClientClaims = new HashSet<ClientClaims>();
            ClientCorsOrigins = new HashSet<ClientCorsOrigins>();
            ClientGrantTypes = new HashSet<ClientGrantTypes>();
            ClientIdPrestrictions = new HashSet<ClientIdPrestrictions>();
            ClientPostLogoutRedirectUris = new HashSet<ClientPostLogoutRedirectUris>();
            ClientProperties = new HashSet<ClientProperties>();
            ClientRedirectUris = new HashSet<ClientRedirectUris>();
            ClientScopes = new HashSet<ClientScopes>();
            ClientSecrets = new HashSet<ClientSecrets>();
            UsersClientsRoles = new HashSet<UsersClientsRoles>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ProtocolType { get; set; }
        public bool RequireClientSecret { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public bool RequirePkce { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public string FrontChannelLogoutUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; }
        public string BackChannelLogoutUri { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int? ConsentLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public int RefreshTokenUsage { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public int AccessTokenType { get; set; }
        public bool EnableLocalLogin { get; set; }
        public bool IncludeJwtId { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public string ClientClaimsPrefix { get; set; }
        public string PairWiseSubjectSalt { get; set; }

        public ICollection<ClientClaims> ClientClaims { get; set; }
        public ICollection<ClientCorsOrigins> ClientCorsOrigins { get; set; }
        public ICollection<ClientGrantTypes> ClientGrantTypes { get; set; }
        public ICollection<ClientIdPrestrictions> ClientIdPrestrictions { get; set; }
        public ICollection<ClientPostLogoutRedirectUris> ClientPostLogoutRedirectUris { get; set; }
        public ICollection<ClientProperties> ClientProperties { get; set; }
        public ICollection<ClientRedirectUris> ClientRedirectUris { get; set; }
        public ICollection<ClientScopes> ClientScopes { get; set; }
        public ICollection<ClientSecrets> ClientSecrets { get; set; }
        public ICollection<UsersClientsRoles> UsersClientsRoles { get; set; }
    }
}
