USE [IdentityServer]
GO

/****** Object:  StoredProcedure [dbo].[CreateClient]    Script Date: 11/15/2018 10:19:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateClient]
	@ClientId NVARCHAR(200),
	@ClientName NVARCHAR(200)
AS

/*LOG
11.15.2018 - ARR - INITIAL CREATION
*/

--SET IDENTITY_INSERT Clients ON

  INSERT INTO Clients ([Enabled], ClientId, ProtocolType, RequireClientSecret, ClientName, [Description], ClientUri, LogoUri, RequireConsent, AllowRememberConsent, AlwaysIncludeUserClaimsInIdToken, RequirePkce, AllowPlainTextPkce, AllowAccessTokensViaBrowser, FrontChannelLogoutUri, FrontChannelLogoutSessionRequired, BackChannelLogoutUri, BackChannelLogoutSessionRequired, AllowOfflineAccess, IdentityTokenLifetime, AccessTokenLifetime, AuthorizationCodeLifetime, ConsentLifetime, AbsoluteRefreshTokenLifetime, SlidingRefreshTokenLifetime, RefreshTokenUsage, UpdateAccessTokenClaimsOnRefresh, RefreshTokenExpiration, AccessTokenType, EnableLocalLogin, IncludeJwtId, AlwaysSendClientClaims, ClientClaimsPrefix, PairWiseSubjectSalt)
	VALUES
( 1, @ClientId,'oidc', 1, @ClientName, NULL, NULL, NULL, 1, 1, 0,	0, 0, 0, NULL, 1, NULL, 1, 1, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0,	1, 0, 1, 0,	0, 'client', NULL)

--SET IDENTITY_INSERT Clients OFF
GO


