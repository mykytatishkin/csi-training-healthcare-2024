﻿CREATE TABLE [dbo].[Employer]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Code] VARCHAR(3) NOT NULL,
	[Name] VARCHAR(30) NOT NULL,
	[State] VARCHAR(30) NOT NULL,
	[Street] VARCHAR(30) NOT NULL,
	[City] VARCHAR(30) NOT NULL,
	[Zip] VARCHAR(9) NOT NULL,
	[Logo] BINARY(50) NOT NULL
)