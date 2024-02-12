﻿CREATE TABLE [dbo].[Claim]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[EmployeeId] INT NOT NULL,
	[ClaimNumber] VARCHAR(50) NOT NULL,
	[DateOfService] DATETIME NOT NULL,
	[PlanId] INT NOT NULL,
	[Amount] DECIMAL NOT NULL,
	[Status] VARCHAR(50) NOT NULL

	CONSTRAINT FK_Claim_Plan_PlanId FOREIGN KEY(PlanId) REFERENCES [dbo].[Plan](Id)
)
