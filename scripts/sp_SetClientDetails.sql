USE [IdentityServer]
GO

/****** Object:  StoredProcedure [dbo].[SetClientDetails]    Script Date: 11/15/2018 10:19:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SetClientDetails]
	@ClientID INT,
	@RedirectUri NVARCHAR(2000),
	@PostLogoutRedirectUris NVARCHAR(2000),
	@AllowedScopes NVARCHAR(200),
	@ClientGrantTypes NVARCHAR(250)
AS

/*LOG
11.15.2018 - ARR - INITIAL CREATION
*/

INSERT INTO ClientRedirectUris (RedirectUri, ClientId)
	VALUES (@RedirectUri, @ClientID)

INSERT INTO ClientPostLogoutRedirectUris (PostLogoutRedirectUri, ClientId)
	VALUES (@PostLogoutRedirectUris, @ClientID)

INSERT INTO ClientScopes (Scope, ClientId)
	VALUES (@AllowedScopes,	@ClientID)

INSERT INTO ClientGrantTypes (GrantType, ClientId)
VALUES (@ClientGrantTypes, @ClientID)
GO


