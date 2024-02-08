CREATE TABLE [dbo].[Settings]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Condition] VARCHAR(30) NOT NULL,
	[State] BIT NOT NULL,
	[EmployerId] INT NOT NULL,

	CONSTRAINT FK_SettingsEmployer FOREIGN KEY (EmployerId) REFERENCES Employer(Id)
)
