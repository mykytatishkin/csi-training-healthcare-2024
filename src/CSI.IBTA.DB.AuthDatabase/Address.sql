﻿CREATE TABLE [dbo].[Address]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[State] VARCHAR(30) NOT NULL,
	[Street] VARCHAR(30) NOT NULL,
	[City] VARCHAR(30) NOT NULL,
	[Zip] VARCHAR(9) NOT NULL,
	[AccountId] INT NOT NULL,

	CONSTRAINT FK_Address_User_AccountId FOREIGN KEY (AccountId) REFERENCES [dbo].[User](Id)
)