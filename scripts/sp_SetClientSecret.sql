USE [IdentityServer]
GO

/****** Object:  StoredProcedure [dbo].[SetClientSecret]    Script Date: 11/15/2018 10:19:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SetClientSecret]
	@SecretHash NVARCHAR(2000),
	@ClientID INT
AS

/*LOG
11.15.2018 - ARR - INITIAL CREATION
*/

INSERT INTO ClientSecrets (Description, Value, Expiration, Type, ClientId)
	VALUES (NULL, @SecretHash, NULL, 'SharedSecret', @ClientID)
GO


