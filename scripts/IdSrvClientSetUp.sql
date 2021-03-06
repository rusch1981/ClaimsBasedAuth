/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 10 *
  FROM [IdentityServer].[dbo].[Clients]

SELECT TOP 10 *
  FROM [IdentityServer].[dbo].[ClientRedirectUris]

  SELECT TOP 10 *
  FROM [IdentityServer].[dbo].[ClientPostLogoutRedirectUris]

  SELECT TOP 10 *
  FROM [IdentityServer].[dbo].[ClientScopes]

  SELECT TOP 10 *
  FROM [IdentityServer].[dbo].[ClientSecrets]

  SELECT TOP 10 *
  FROM [IdentityServer].[dbo].[ClientGrantTypes]

  SELECT TOP 10 *
  FROM [IdentityServer].[dbo].[ClientScopes]

  SELECT TOP 100 *
  FROM [IdentityServer].[dbo].[UsersClientsRoles]

  /*************Create a Client******************/
GO

SET IDENTITY_INSERT Clients ON

  INSERT INTO Clients (Id, [Enabled], ClientId, ProtocolType, RequireClientSecret, ClientName, [Description], ClientUri, LogoUri, RequireConsent, AllowRememberConsent, AlwaysIncludeUserClaimsInIdToken, RequirePkce, AllowPlainTextPkce, AllowAccessTokensViaBrowser, FrontChannelLogoutUri, FrontChannelLogoutSessionRequired, BackChannelLogoutUri, BackChannelLogoutSessionRequired, AllowOfflineAccess, IdentityTokenLifetime, AccessTokenLifetime, AuthorizationCodeLifetime, ConsentLifetime, AbsoluteRefreshTokenLifetime, SlidingRefreshTokenLifetime, RefreshTokenUsage, UpdateAccessTokenClaimsOnRefresh, RefreshTokenExpiration, AccessTokenType, EnableLocalLogin, IncludeJwtId, AlwaysSendClientClaims, ClientClaimsPrefix, PairWiseSubjectSalt)
	VALUES
(2, 1,'mvc2','oidc', 1,'MVC Client II', NULL, NULL, NULL, 1, 1, 0,	0, 0, 0, NULL, 1, NULL, 1, 1, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0,	1, 0, 1, 0,	0, 'client', NULL)

SET IDENTITY_INSERT Clients OFF


GO

SET IDENTITY_INSERT ClientRedirectUris ON

INSERT INTO ClientRedirectUris (Id,	RedirectUri,ClientId)
	VALUES (2,'https://localhost:44330',2)

SET IDENTITY_INSERT ClientRedirectUris OFF

GO

SET IDENTITY_INSERT ClientPostLogoutRedirectUris ON

INSERT INTO ClientPostLogoutRedirectUris (Id, PostLogoutRedirectUri, ClientId)
	VALUES (2, 'https://localhost:44330', 2)

SET IDENTITY_INSERT ClientPostLogoutRedirectUris OFF

GO

SET IDENTITY_INSERT ClientScopes ON

INSERT INTO ClientScopes (Id, Scope, ClientId)
	VALUES (5,'NEW BLAH',	2)

SET IDENTITY_INSERT ClientScopes OFF

GO

SET IDENTITY_INSERT ClientSecrets ON

INSERT INTO ClientSecrets (Id, Description, Value, Expiration, Type, ClientId)
	VALUES (2, NULL, 'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=', NULL, 'SharedSecret', 2)

SET IDENTITY_INSERT ClientSecrets OFF

GO

SET IDENTITY_INSERT ClientGrantTypes ON

INSERT INTO ClientGrantTypes (Id, GrantType, ClientId)
VALUES (3,'client_credentials', 2)

SET IDENTITY_INSERT ClientGrantTypes OFF

GO

--SET IDENTITY_INSERT ClientScopes ON

--INSERT INTO ClientScopes (Id, Scope, ClientId)
--VALUES (8, 'customProfile2',	2)

--SET IDENTITY_INSERT ClientScopes OFF

--GO


INSERT INTO UsersClientsRoles (Id, UserId, ClientId, UserRoleId)
VALUES (8, 2, 2, 1)

GO
