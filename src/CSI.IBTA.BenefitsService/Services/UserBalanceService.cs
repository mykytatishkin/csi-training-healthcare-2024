using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using System.Net;

namespace CSI.IBTA.BenefitsService.Services
{
    internal class UserBalanceService : IUserBalanceService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;

        public UserBalanceService(IBenefitsUnitOfWork benefitsUnitOfWork)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
        }

        public async Task<GenericResponse<decimal>> GetCurrentBalance(int enrollmentId)
        {
            var enrollment = await _benefitsUnitOfWork.Enrollments
                .Include(c => c.Plan)
                .Include(c => c.Plan.Package)
                .FirstOrDefaultAsync(x => x.Id == enrollmentId);

            if (enrollment == null) return new GenericResponse<decimal>(HttpErrors.ResourceNotFound, 0);

            var package = enrollment.Plan.Package;
            if (!package.IsActive) return new GenericResponse<decimal>(new HttpError("This package is not active yet", HttpStatusCode.BadRequest), 0);

            var transactions = await _benefitsUnitOfWork.Transactions
                .Include(x => x.Enrollment)
                .Where(x => x.Enrollment.Id == enrollmentId)
                .ToListAsync();

            var balance = transactions.Sum(x => x.Type == TransactionType.Income ? x.Amount : -x.Amount);
            return new GenericResponse<decimal>(null, balance);
        }

        public async Task<GenericResponse<List<string>>> ImportContributions(List<string> contributionStrings)
        {
            //Dictionary<int, string> columnsMap = new() {
            //    { 0, "Employee SSN" },
            //    { 1, "Employee plan name" },
            //    { 2, "Contribution amount" },
            //};

            //List<string> errors = [];
            //List<ContributionDtos> contributions = [];

            //for (int i = 0; i < contributionStrings.Count; i++)
            //{
            //    int recordNumber = i + 1;
            //    string line = contributionStrings[i];

            //    Console.WriteLine($"Record #{recordNumber}: {line}");
            //    var content = line.Split(',');

            //    if (content.Length != 3)
            //    {
            //        Console.WriteLine($"The import file should contain only the records: {string.Join(',', columnsMap.Values)}");
            //        return new(null, [$"The import file should contain only the records: {string.Join(',', columnsMap.Values)}"]);
            //    }

            //    for (int j = 0; j < content.Length; j++)
            //    {
            //        if (string.IsNullOrEmpty(content[j]))
            //        {
            //            Console.WriteLine($"Record #{recordNumber} failed because {columnsMap[j]} is empty");
            //            errors.Add($"Record #{recordNumber} failed because {columnsMap[j]} is empty");
            //        }
            //    }

            //    if (decimal.TryParse(content[2], out decimal contribution))
            //    {
            //        Console.WriteLine($"Record #{recordNumber} failed because {columnsMap[2]} is not a number");
            //        errors.Add($"Record #{recordNumber} failed because {columnsMap[2]} is not a number");
            //    }

            //    if (contribution < 0)
            //    {
            //        Console.WriteLine($"Record #{recordNumber} failed because {columnsMap[2]} is not a positive number");
            //        errors.Add($"Record #{recordNumber} failed because {columnsMap[2]} is not a positive number");
            //    }

            //    string employeeSsn = content[0];
            //    string planName = content[1];

            //    var plan = _benefitsUnitOfWork.Plans.Find(p => p.Name == planName);
            //    var enrollment = _benefitsUnitOfWork.Enrollments.Find(e => e.Plan.Id == plan.Id && e.EmployeeId ==)

            //    var contributionEntry = new ContributionDtos
            //    {
            //        SSN = content[0],
            //        PlanName = content[1],
            //        Contribution = contribution,
            //    };

            //    contributions.Add(contributionEntry);

            //    recordNumber++;
            //}

            return new(null, []);
        }
    }
}