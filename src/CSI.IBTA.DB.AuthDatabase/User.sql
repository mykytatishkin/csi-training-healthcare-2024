CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[FirstName] VARCHAR(30) NOT NULL,
	[LastName] VARCHAR(30) NOT NULL,
	[AccountId] INT NOT NULL,

	CONSTRAINT FK_User_Account_AccountId FOREIGN KEY (AccountId) REFERENCES [dbo].[Account](Id)
)
