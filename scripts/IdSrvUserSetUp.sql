/****** Check Tables  ******/
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


/****** Remove Users and Role Associations ******/
  GO

  DELETE FROM UsersClientsRoles
  DELETE FROM Users
  DELETE FROM UsersRoles
  
  
/****** Add Roles ******/
 
  INSERT INTO UsersRoles(Role)
  VALUES ('admin')
  INSERT INTO UsersRoles(Role)
  VALUES ('legend')
  INSERT INTO UsersRoles(Role)
  VALUES ('noob')

/****** Add Users ******/
  GO 

  INSERT INTO Users (Id, UserName)
  VALUES (1, 'arusch')

  INSERT INTO Users (Id, UserName)
  VALUES (2, 'jlipford')

  /****** Add  Role Associations ******/

  INSERT INTO UsersClientsRoles
  VALUES (1, 1, 1, 3)

  INSERT INTO UsersClientsRoles
  VALUES (2, 1, 1, 1)

  INSERT INTO UsersClientsRoles
  VALUES (3, 2, 1, 2)

  INSERT INTO UsersClientsRoles
  VALUES (4, 2, 1, 1)



  /****** Object:  Table [dbo].[Users]    Script Date: 11/8/2018 11:53:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Inserted]  DEFAULT (getdate()) FOR [DateCreated]
GO

/****** Object:  Table [dbo].[UsersRoles]    Script Date: 11/8/2018 11:54:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UsersRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
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
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClientId] [int] NOT NULL,
	[UserRoleId] [int] NOT NULL,
 CONSTRAINT [PK_UsersClientsRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UsersClientsRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersClientsRoles_Clilents_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO

ALTER TABLE [dbo].[UsersClientsRoles] CHECK CONSTRAINT [FK_UsersClientsRoles_Clilents_ClientId]
GO

ALTER TABLE [dbo].[UsersClientsRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersClientsRoles_Users_UserId_] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([ID])
GO

ALTER TABLE [dbo].[UsersClientsRoles] CHECK CONSTRAINT [FK_UsersClientsRoles_Users_UserId_]
GO

ALTER TABLE [dbo].[UsersClientsRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersClientsRoles_UsersRoles_UserRoleId] FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UsersRoles] ([Id])
GO

ALTER TABLE [dbo].[UsersClientsRoles] CHECK CONSTRAINT [FK_UsersClientsRoles_UsersRoles_UserRoleId]
GO

GO