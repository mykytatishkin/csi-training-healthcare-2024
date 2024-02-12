CREATE TABLE [dbo].[Phone]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Phone] VARCHAR(30) NOT NULL,
	[AccountId] INT NOT NULL,

	CONSTRAINT FK_Phone_User_AccountId FOREIGN KEY (AccountId) REFERENCES [dbo].[User](Id)
)
