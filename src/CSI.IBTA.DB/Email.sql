CREATE TABLE [dbo].[Email]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Email] VARCHAR(60) NOT NULL,
	[AccountId] INT NOT NULL,

	CONSTRAINT FK_Email_User_AccountId FOREIGN KEY (AccountId) REFERENCES [dbo].[User](Id)
)
