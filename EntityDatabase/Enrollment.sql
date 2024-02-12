CREATE TABLE [dbo].[Enrollment]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Election] DECIMAL NOT NULL,
	[EmployeeId] INT NOT NULL,
	[PlanId] INT NOT NULL,

	CONSTRAINT FK_Enrollment_Plan_PlanId FOREIGN KEY(PlanId) REFERENCES [dbo].[Plan](Id)
)
