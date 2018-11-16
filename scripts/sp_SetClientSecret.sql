USE [IdentityServer]
GO

/****** Object:  StoredProcedure [dbo].[SetClientSecret]    Script Date: 11/15/2018 11:53:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SetClientSecret]
	@INPUT_SecretHash NVARCHAR(2000),
	@INPUT_ClientID INT
AS

/*LOG
11.15.2018 - ARR - INITIAL CREATION
*/

DECLARE
	@Description NVARCHAR(2000) = NULL, 
	@Expiration DATETIME2(7) = NULL, 
	@Type NVARCHAR(250) = 'SharedSecret'

IF EXISTS ( SELECT * FROM Clients WHERE Id = @INPUT_ClientID)
	BEGIN
		IF ( @INPUT_SecretHash IS NULL OR @INPUT_SecretHash = '')
				BEGIN
					PRINT N'NO SecretHash was PROVIDED.  NO ACTION WAS ON THE ClientSecrets TABLE';
				END		
		ELSE
			BEGIN	
				INSERT INTO ClientSecrets (ClientId, Value, [Description], Expiration, [Type])
					VALUES (@INPUT_ClientID, @INPUT_SecretHash, @Description, @Expiration, @Type)
			END
	END
ELSE
	PRINT N'THE CLEINT ID (' + CAST(@INPUT_ClientID AS VARCHAR) +') DOES NOT EXIST ON THE Clients TABLE.';
GO


