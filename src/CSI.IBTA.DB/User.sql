CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[FirstName] VARCHAR(30) NOT NULL,
	[LastName] VARCHAR(30) NOT NULL,
	[AccountId] INT NOT NULL,

	CONSTRAINT FK_UserAccount FOREIGN KEY (AccountId) REFERENCES [dbo].[Account](Id)
)
