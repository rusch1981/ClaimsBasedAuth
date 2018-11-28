USE [IdentityServer]
GO

/****** Object:  StoredProcedure [dbo].[CreateClient]    Script Date: 11/15/2018 11:51:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateClient]
	@INPUT_ClientId NVARCHAR(200),
	@INPUT_ClientName NVARCHAR(200)
AS

/*LOG
11.15.2018 - ARR - INITIAL CREATION
*/

DECLARE 
	@Enabled BIT = 1,
	@ProtocolType NVARCHAR(200) = 'oidc',
	@RequireClientSecret BIT = 1,
	@Description NVARCHAR(1000) = NULL,
	@ClientUri NVARCHAR(2000) = NULL,
	@LogoUri NVARCHAR(2000) = NULL,
	@RequireConsent BIT = 1,
	@AllowRememberConsent BIT = 1,
	@AlwaysIncludeUserClaimsInIdToken BIT = 1, 
	@RequirePkce BIT = 0, 
	@AllowPlainTextPkce BIT = 0, 
	@AllowAccessTokensViaBrowser BIT = 0, 
	@FrontChannelLogoutUri NVARCHAR(2000) = NULL, 
	@FrontChannelLogoutSessionRequired BIT = 1, 
	@BackChannelLogoutUri NVARCHAR(2000) = NULL, 
	@BackChannelLogoutSessionRequired BIT = 1, 
	@AllowOfflineAccess BIT = 1, 
	@IdentityTokenLifetime INT = 300, 
	@AccessTokenLifetime INT = 3600, 
	@AuthorizationCodeLifetime INT = 300, 
	@ConsentLifetime INT = NULL, 
	@AbsoluteRefreshTokenLifetime INT = 2592000, 
	@SlidingRefreshTokenLifetime INT = 1296000, 
	@RefreshTokenUsage INT = 1, 
	@UpdateAccessTokenClaimsOnRefresh BIT = 0, 
	@RefreshTokenExpiration INT = 1, 
	@AccessTokenType INT = 0, 
	@EnableLocalLogin BIT = 1, 
	@IncludeJwtId BIT = 0, 
	@AlwaysSendClientClaims BIT = 0, 
	@ClientClaimsPrefix NVARCHAR(200) = 'client', 
	@PairWiseSubjectSalt NVARCHAR(200) = NULL

IF EXISTS (SELECT * FROM Clients WHERE ClientId = @INPUT_ClientId)  
	PRINT N'THE CLIENTID (' + @INPUT_ClientId +') HAS ALREADY BEEN CREATED';

ELSE IF EXISTS (SELECT * FROM Clients WHERE ClientName = @INPUT_ClientName)  
	PRINT N'THE CLIENTNAME (' + @INPUT_ClientName +') HAS ALREADY BEEN CREATED';

ELSE
  INSERT INTO Clients ( ClientId, ClientName, [Enabled], ProtocolType, RequireClientSecret,  [Description], ClientUri, LogoUri, RequireConsent, AllowRememberConsent, AlwaysIncludeUserClaimsInIdToken, RequirePkce, AllowPlainTextPkce, AllowAccessTokensViaBrowser, FrontChannelLogoutUri, FrontChannelLogoutSessionRequired, BackChannelLogoutUri, BackChannelLogoutSessionRequired, AllowOfflineAccess, IdentityTokenLifetime, AccessTokenLifetime, AuthorizationCodeLifetime, ConsentLifetime, AbsoluteRefreshTokenLifetime, SlidingRefreshTokenLifetime, RefreshTokenUsage, UpdateAccessTokenClaimsOnRefresh, RefreshTokenExpiration, AccessTokenType, EnableLocalLogin, IncludeJwtId, AlwaysSendClientClaims, ClientClaimsPrefix, PairWiseSubjectSalt)
	VALUES
( @INPUT_ClientId, @INPUT_ClientName, @Enabled, @ProtocolType, @RequireClientSecret, @Description, @ClientUri, @LogoUri, @RequireConsent, @AllowRememberConsent, @AlwaysIncludeUserClaimsInIdToken,	@RequirePkce, @AllowPlainTextPkce, @AllowAccessTokensViaBrowser, @FrontChannelLogoutUri, @FrontChannelLogoutSessionRequired, @BackChannelLogoutUri, @BackChannelLogoutSessionRequired, @AllowOfflineAccess, @IdentityTokenLifetime, @AccessTokenLifetime, @AuthorizationCodeLifetime, @ConsentLifetime, @AbsoluteRefreshTokenLifetime, @SlidingRefreshTokenLifetime, @RefreshTokenUsage, @UpdateAccessTokenClaimsOnRefresh,	@RefreshTokenExpiration, @AccessTokenType, @EnableLocalLogin, @IncludeJwtId,	@AlwaysSendClientClaims, @ClientClaimsPrefix, @PairWiseSubjectSalt)


GO


