USE [IdentityServer]
GO

/****** Object:  StoredProcedure [dbo].[SetClientDetails]    Script Date: 11/15/2018 11:53:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SetClientDetails]
	@INPUT_ClientID INT,
	@INPUT_ClientGrantTypes NVARCHAR(250),
	@INPUT_RedirectUri NVARCHAR(2000) = NULL,
	@INPUT_PostLogoutRedirectUris NVARCHAR(2000) = NULL,
	@INPUT_Scopes NVARCHAR(200) = NULL
	
AS

/*LOG
11.15.2018 - ARR - INITIAL CREATION
*/

IF EXISTS ( SELECT * FROM Clients WHERE Id = @INPUT_ClientID)
	BEGIN
		IF ( @INPUT_RedirectUri IS NULL OR @INPUT_RedirectUri = '')
			BEGIN
				PRINT N'THE RedirectUri WAS NOT PROVIDED.  NO ACTION WAS ON THE ClientRedirectUris TABLE';
			END
		ELSE
			BEGIN			
				INSERT INTO ClientRedirectUris (RedirectUri, ClientId)
					VALUES (@INPUT_RedirectUri, @INPUT_ClientID)
			END

		IF ( @INPUT_PostLogoutRedirectUris IS NULL OR @INPUT_PostLogoutRedirectUris = '')
			BEGIN
				PRINT N'THE PostLogoutRedirectUri WAS NOT PROVIDED.  NO ACTION WAS ON THE ClientPostLogoutRedirectUris TABLE';
			END
		ELSE
			BEGIN			
				INSERT INTO ClientPostLogoutRedirectUris (PostLogoutRedirectUri, ClientId)
					VALUES (@INPUT_PostLogoutRedirectUris, @INPUT_ClientID)
			END

		IF ( @INPUT_Scopes IS  NULL OR @INPUT_Scopes = '')
			BEGIN
				PRINT N'NO Scopes WERE PROVIDED.  NO ACTION WAS ON THE ClientScopes TABLE';
			END
		ELSE
			BEGIN			
				INSERT INTO ClientScopes (Scope, ClientId)
					VALUES (@INPUT_Scopes,	@INPUT_ClientID)
			END

		IF ( @INPUT_ClientGrantTypes IS NULL OR @INPUT_ClientGrantTypes = '')
			BEGIN
				PRINT N'NO GrantTypes WERE PROVIDED.  NO ACTION WAS ON THE ClientGrantTypes TABLE';
			END
		ELSE
			BEGIN			
				INSERT INTO ClientGrantTypes (GrantType, ClientId)
				VALUES (@INPUT_ClientGrantTypes, @INPUT_ClientID)
			END
	END 
ELSE
	PRINT N'THE CLEINT ID (' + CAST(@INPUT_ClientID AS VARCHAR) +') DOES NOT EXIST ON THE Clients TABLE.';

GO


