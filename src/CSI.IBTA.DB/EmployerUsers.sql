CREATE TABLE [dbo].[EmployerUsers]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[UserId] INT NOT NULL,
	[EmployerId] INT NOT NULL,

	CONSTRAINT FK_EmpUser_UserId FOREIGN KEY (UserId) REFERENCES [dbo].[User](Id),
	CONSTRAINT FK_EmpUser_EmployerId FOREIGN KEY (EmployerId) REFERENCES [dbo].[Employer](Id)
)
