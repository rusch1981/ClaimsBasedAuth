USE [IdentityServer]
GO

/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 11/15/2018 10:18:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CreateUser]
	@UserName NVARCHAR(50),
	@ClientID INT,
	@UserRoleId INT
	
AS

/*LOG
11.15.2018 - ARR - INITIAL CREATION
*/

INSERT INTO Users (UserName)
  VALUES (@UserName)

DECLARE @UserID INT;

SET @UserID = (SELECT TOP 1 Id FROM Users WHERE UserName = @UserName)

INSERT INTO UsersClientsRoles (UserId, ClientId, UserRoleId)
  VALUES (@UserID, @ClientID, @UserRoleId)
GO