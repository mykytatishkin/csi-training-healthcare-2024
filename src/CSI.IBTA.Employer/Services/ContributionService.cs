using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Types;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Services
{
    public class ContributionService : IContributionService
    {
        private readonly IEmployeesClient _employeesClient;
        private readonly IPlansClient _plansClient;
        private readonly IEnrollmentsClient _enrollmentsClient;

        public ContributionService(IEmployeesClient employeesClient, IPlansClient plansClient, IEnrollmentsClient enrollmentsClient)
        {
            _employeesClient = employeesClient;
            _plansClient = plansClient;
            _enrollmentsClient = enrollmentsClient;
        }

        public async Task<GenericResponse<ContributionsResponse>> ProcessContributionsFile(IFormFile file)
        {
            Dictionary<int, string> columnsMap = new() {
                { 0, "Employee SSN" },
                { 1, "Employee plan name" },
                { 2, "Contribution amount" },
            };

            Dictionary<int, List<string>> errors = [];
            List<UnprocessedContributionDto> unprocessedContributions = [];

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                int recordNumber = 1;
                string? line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    if (!errors.ContainsKey(recordNumber))
                    {
                        errors.Add(recordNumber, []);
                    }

                    var content = line.Split(',');

                    if (content.Length != 3)
                    {
                        var invalidFileResponse = new Dictionary<int, List<string>> { { -1, [$"The import file should contain only the records: {string.Join(", ", columnsMap.Values)}"] } };
                        return new(null, new() { Errors = invalidFileResponse });
                    }

                    for (int i = 0; i < content.Length; i++)
                    {
                        if (string.IsNullOrEmpty(content[i]))
                        {
                            errors[recordNumber].Add($"Record #{recordNumber} failed because {columnsMap[i]} is empty");
                        }
                    }

                    if (!decimal.TryParse(content[2], out decimal contribution))
                    {
                        errors[recordNumber].Add($"Record #{recordNumber} failed because {columnsMap[2]} is not a number");
                    }

                    if (contribution < 0)
                    {
                        Console.WriteLine($"Record #{recordNumber} failed because {columnsMap[2]} is negative");
                        errors[recordNumber].Add($"Record #{recordNumber} failed because {columnsMap[2]} is negative");
                    }

                    var contributionEntry = new UnprocessedContributionDto
                    (
                        recordNumber,
                        content[0],
                        content[1],
                        contribution
                    );

                    unprocessedContributions.Add(contributionEntry);

                    recordNumber++;
                }
            }

            var ssns = unprocessedContributions.Select(c => c.SSN).Distinct().ToList();
            var usersResponse = await _employeesClient.GetUsersBySSNs(ssns);

            if (usersResponse.Error != null)
            {
                return new(usersResponse.Error, null);
            }

            var users = usersResponse.Result;
            var userIds = users!.Select(users => users.Id).Distinct().ToList();

            var planNames = unprocessedContributions.Select(c => c.PlanName).Distinct().ToList();
            var plansResponse = await _plansClient.GetPlansByNames(planNames);

            if (plansResponse.Error != null)
            {
                return new(plansResponse.Error, null);
            }

            var plans = plansResponse.Result;

            var enrollmentsResponse = await _enrollmentsClient.GetEnrollmentsByUserIds(userIds);

            if (enrollmentsResponse.Error != null)
            {
                return new(enrollmentsResponse.Error, null);
            }

            var enrollments = enrollmentsResponse.Result;

            List<ProcessedContributionDto> processedContributions = [];

            foreach (var unprocessedContribution in unprocessedContributions)
            {
                bool valuesExistInDb = true;

                UserDto? user = null;
                if (!string.IsNullOrEmpty(unprocessedContribution.SSN))
                {
                    user = users!.Where(u => u.SSN == unprocessedContribution.SSN).FirstOrDefault();

                    if (user == null)
                    {
                        errors[unprocessedContribution.RecordNumber].Add($"Record #{unprocessedContribution.RecordNumber} failed because {columnsMap[0]} does not exist in DB");
                        valuesExistInDb = false;
                    }
                }

                PlanDto? plan = null;
                if (!string.IsNullOrEmpty(unprocessedContribution.PlanName))
                {
                    plan = plans!.Where(p => p.Name == unprocessedContribution.PlanName).FirstOrDefault();

                    if (plan == null)
                    {
                        errors[unprocessedContribution.RecordNumber].Add($"Record #{unprocessedContribution.RecordNumber} failed because {columnsMap[1]} does not exist in DB or is inactive");
                        valuesExistInDb = false;
                    }
                }

                if (user != null && plan != null)
                {
                    EnrollmentDto? enrollment = enrollments!.Where(e => e.PlanId == plan.Id && e.EmployeeId == user.Id).FirstOrDefault();

                    if (enrollment == null)
                    {
                        errors[unprocessedContribution.RecordNumber].Add($"Record #{unprocessedContribution.RecordNumber} failed because {columnsMap[0]} is not enrolled into specified plan");
                        valuesExistInDb = false;
                    }
                }

                if (valuesExistInDb == false)
                {
                    continue;
                }

                int userId = user!.Id;
                int planId = plan!.Id;

                // ProcessedContributionDto has enrollment and contribution amount?
                var processedContribution = new ProcessedContributionDto(userId, planId, unprocessedContribution.Contribution);
                processedContributions.Add(processedContribution);
            }

            return new(null, new ContributionsResponse() { ProcessedContributions = processedContributions, Errors = errors });
        }
    }
}