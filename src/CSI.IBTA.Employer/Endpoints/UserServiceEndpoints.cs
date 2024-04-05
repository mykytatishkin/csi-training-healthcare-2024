namespace CSI.IBTA.Employer.Endpoints
{
    public class UserServiceEndpoints
    {
        public const string UsersByUsernames = "v1/UsersByUsernames";

        public const string Employees = "v1/Employee?page={0}&pageSize={1}&employerId={2}&firstname={3}&lastname={4}&ssn={5}";
        public const string CreateEmployee = "v1/Employee";

        public const string Employer = "v1/Employer";
        public const string GetEmployerByAccountId = "v1/Employer/GetByAccountId";
    }
}
