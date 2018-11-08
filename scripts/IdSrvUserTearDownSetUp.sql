/****** Script for SelectTopNRows command from SSMS  ******/
GO

SELECT TOP 1000 *
  FROM [IdentityServer].[dbo].[Users]

  SELECT TOP 1000 [Id]
      ,[Role]
  FROM [IdentityServer].[dbo].[UsersRoles]

  SELECT TOP 1000 [Id]
      ,[UserId]
      ,[ClientId]
      ,[UserRoleId]
  FROM [IdentityServer].[dbo].[UsersClientsRoles]



  /*----------------- Remove Users and Role Associations */
  GO
  
  DELETE FROM UsersClientsRoles
  DELETE FROM Users
  

  /*------------------Replace Users and Role Associations */
  GO 

  INSERT INTO Users (Id, UserName)
  VALUES (1, 'arusch')

  INSERT INTO Users (Id, UserName)
  VALUES (2, 'jlipford')

  INSERT INTO UsersClientsRoles
  VALUES (1, 1, 1, 3)

  INSERT INTO UsersClientsRoles
  VALUES (2, 1, 1, 1)

  INSERT INTO UsersClientsRoles
  VALUES (3, 2, 1, 2)

  INSERT INTO UsersClientsRoles
  VALUES (4, 2, 1, 1)

  /*----------------------ADD Default Val*/
  
  GO

  ALTER TABLE Users
  ADD CONSTRAINT DF_Users_Inserted
  DEFAULT GETDATE()
  FOR DateCreated


  /****** Object:  Table [dbo].[Users]    Script Date: 11/8/2018 11:53:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_Users_Inserted]  DEFAULT (getdate()),
	[Description] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[UsersRoles]    Script Date: 11/8/2018 11:54:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UsersRoles](
	[Id] [int] NOT NULL,
	[Role] [nchar](250) NOT NULL,
 CONSTRAINT [PK_UsersRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[UsersClientsRoles]    Script Date: 11/8/2018 11:54:41 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UsersClientsRoles](
	[Id] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ClientId] [int] NOT NULL,
	[UserRoleId] [int] NOT NULL
) ON [PRIMARY]

GO