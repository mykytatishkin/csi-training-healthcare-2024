﻿CREATE TABLE [dbo].[Account]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Username] VARCHAR(24) NOT NULL,
	[Password] VARCHAR(32) NOT NULL,
	[RoleId] INT NOT NULL,

	CONSTRAINT FK_Account_Role_RoleId FOREIGN KEY (RoleId) REFERENCES [dbo].[Role](Id)
)
