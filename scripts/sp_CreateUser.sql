USE [IdentityServer]
GO

/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 11/15/2018 11:52:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CreateUser]
	@INPUT_UserName NVARCHAR(50),
	@INPUT_Description NVARCHAR(MAX) = NULL
AS

/*LOG
11.15.2018 - ARR - INITIAL CREATION
*/

IF EXISTS (SELECT * FROM Users WHERE UserName = @INPUT_UserName)
	PRINT N'THE USER NAME (' + @INPUT_UserName +') HAS ALREADY BEEN CREATED';
ELSE

INSERT INTO Users (UserName, Description)
  VALUES (@INPUT_UserName, @INPUT_Description)

GO