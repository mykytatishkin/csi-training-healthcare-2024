CREATE TABLE [dbo].[EmployerUsers]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[UserId] INT NOT NULL,
	[EmployerId] INT NOT NULL,

	CONSTRAINT FK_EmployerUsers_User_UserId FOREIGN KEY (UserId) REFERENCES [dbo].[User](Id),
	CONSTRAINT FK_EmployerUsers_Employer_EmployerId FOREIGN KEY (EmployerId) REFERENCES [dbo].[Employer](Id)
)
