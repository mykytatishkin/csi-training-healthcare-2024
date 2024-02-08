CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[FirstName] VARCHAR(30) NOT NULL,
	[LastName] VARCHAR(30) NOT NULL,
	[AccountId] INT NOT NULL,
	[EmployerId] INT NOT NULL,

	CONSTRAINT FK_UserAccount FOREIGN KEY (AccountId) REFERENCES Account(Id),
	CONSTRAINT FK_UserEmployer FOREIGN KEY (EmployerId) REFERENCES Employer(Id)
	
)
