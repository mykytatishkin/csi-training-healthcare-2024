﻿CREATE TABLE [dbo].[Account]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Username] VARCHAR(24) NOT NULL,
	[Password] VARCHAR(32) NOT NULL,
	[Role] INT NOT NULL
)