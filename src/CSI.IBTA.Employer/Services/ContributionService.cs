using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Types;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Services
{
    public class ContributionService : IContributionsService
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
                { 0, "Employee username" },
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
                        List<string> invalidFileResponse = [$"The import file should contain only the records: {string.Join(", ", columnsMap.Values)}"];
                        return new(null, new() { Errors = invalidFileResponse });
                    }

                    ValidateContributionLine(content, recordNumber, columnsMap, ref errors, out decimal contribution);

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

            var response = await GetDataFromDatabase(unprocessedContributions);

            if (response.Error != null)
            {
                return new(response.Error, null);
            }

            (List<UserDto> users, List<PlanDto> plans, List<EnrollmentDto> enrollments) = response.Result;

            List<ProcessedContributionDto> processedContributions = GetProcessedContributions(
                unprocessedContributions, 
                users, 
                plans, 
                enrollments,
                columnsMap,
                ref errors);

            var orderedErrors = errors.OrderBy(e => e.Key).SelectMany(e => e.Value).ToList();

            return new(null, new ContributionsResponse()
            {
                ProcessedContributions = processedContributions,
                Errors = orderedErrors
            });
        }

        private void ValidateContributionLine(
            string[] content,
            int recordNumber,
            Dictionary<int, string> columnsMap,
            ref Dictionary<int, List<string>> errors,
            out decimal contribution)
        {
            for (int i = 0; i < content.Length; i++)
            {
                if (string.IsNullOrEmpty(content[i]))
                {
                    errors[recordNumber].Add($"Record #{recordNumber} failed because {columnsMap[i]} is empty");
                }
            }

            if (!decimal.TryParse(content[2], out contribution))
            {
                errors[recordNumber].Add($"Record #{recordNumber} failed because {columnsMap[2]} is not a number");
            }

            if (contribution < 0)
            {
                errors[recordNumber].Add($"Record #{recordNumber} failed because {columnsMap[2]} is negative");
            }
        }

        private async Task<GenericResponse<(List<UserDto>, List<PlanDto>, List<EnrollmentDto>)>> GetDataFromDatabase(
            List<UnprocessedContributionDto> unprocessedContributions)
        {
            var usernames = unprocessedContributions.Select(c => c.Username).Distinct().ToList();
            var usersResponse = await _employeesClient.GetUsersByUsernames(usernames);

            if (usersResponse.Error != null)
            {
                return new(usersResponse.Error, (null, null, null)!);
            }

            var users = usersResponse.Result;

            var planNames = unprocessedContributions.Select(c => c.PlanName).Distinct().ToList();
            var plansResponse = await _plansClient.GetPlansByNames(planNames);

            if (plansResponse.Error != null)
            {
                return new(plansResponse.Error, (null, null, null)!);
            }

            var plans = plansResponse.Result;

            var userIds = users!.Select(users => users.Id).Distinct().ToList();
            var enrollmentsResponse = await _enrollmentsClient.GetEnrollmentsByUserIds(userIds);

            if (enrollmentsResponse.Error != null)
            {
                return new(enrollmentsResponse.Error, (null, null, null)!);
            }

            var enrollments = enrollmentsResponse.Result;

            return new(null, (users!.ToList(), plans!.ToList(), enrollments!.ToList()));
        }

        private List<ProcessedContributionDto> GetProcessedContributions(
            List<UnprocessedContributionDto> unprocessedContributions, 
            List<UserDto> users,
            List<PlanDto> plans,
            List<EnrollmentDto> enrollments,
            Dictionary<int, string> columnsMap,
            ref Dictionary<int, List<string>> errors)
        {
            List<ProcessedContributionDto> processedContributions = [];

            foreach (var unprocessedContribution in unprocessedContributions)
            {
                int recordNumber = unprocessedContribution.RecordNumber;
                bool valuesExistInDb = true;

                UserDto? user = null;
                if (!string.IsNullOrEmpty(unprocessedContribution.Username))
                {
                    user = users!.FirstOrDefault(u => u.UserName == unprocessedContribution.Username);

                    if (user == null)
                    {
                        errors[recordNumber].Add($"Record #{recordNumber} failed because {columnsMap[0]} does not exist in DB");
                        valuesExistInDb = false;
                    }
                }

                PlanDto? plan = null;
                if (!string.IsNullOrEmpty(unprocessedContribution.PlanName))
                {
                    plan = plans!.FirstOrDefault(p => p.Name == unprocessedContribution.PlanName);

                    if (plan == null)
                    {
                        errors[recordNumber].Add($"Record #{recordNumber} failed because {columnsMap[1]} does not exist in DB or is inactive");
                        valuesExistInDb = false;
                    }
                }

                EnrollmentDto? enrollment = null;
                if (user != null && plan != null)
                {
                    enrollment = enrollments!.FirstOrDefault(e => e.PlanId == plan.Id && e.EmployeeId == user.Id);

                    if (enrollment == null)
                    {
                        errors[recordNumber].Add($"Record #{recordNumber} failed because {columnsMap[0]} is not enrolled into specified plan");
                        valuesExistInDb = false;
                    }
                }

                if (valuesExistInDb == false)
                {
                    continue;
                }

                int userId = user!.Id;
                int planId = plan!.Id;
                int enrollmentId = enrollment!.Id;

                var processedContribution = new ProcessedContributionDto(enrollmentId, unprocessedContribution.Contribution);
                processedContributions.Add(processedContribution);
            }

            return processedContributions;
        }
    }
}