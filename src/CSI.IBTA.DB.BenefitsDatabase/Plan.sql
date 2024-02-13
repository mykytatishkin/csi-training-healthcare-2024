CREATE TABLE [dbo].[Plan]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[TypeId] INT NOT NULL,
	[EmployeeId] INT NOT NULL,
	[Status] VARCHAR(30) NOT NULL,
	[Contribution] DECIMAL NOT NULL,
	[PackageId] INT NOT NULL

	CONSTRAINT FK_Plan_Type_TypeId FOREIGN KEY (TypeId) REFERENCES [dbo].[Type](Id),
	CONSTRAINT FK_Plan_Package_PackageId FOREIGN KEY(PackageId) REFERENCES [dbo].[Package](Id)
)
