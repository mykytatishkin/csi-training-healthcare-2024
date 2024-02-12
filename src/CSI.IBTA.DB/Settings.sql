CREATE TABLE [dbo].[Settings]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Condition] VARCHAR(30) NOT NULL,
	[State] BIT NOT NULL,
	[EmployerId] INT NOT NULL,

	CONSTRAINT FK_Settings_Employer_EmployerId FOREIGN KEY (EmployerId) REFERENCES [dbo].[Employer](Id)
)
