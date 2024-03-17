namespace CSI.IBTA.Administrator.Endpoints
{
    public class UserServiceApiEndpoints
    {
        public const string UsersByIds = "v1/Users";
        public const string Users = "v1/User";
        public const string User = "v1/User/{0}";

        public const string EmployersByIds = "v1/Employers";
        public const string Employers = "v1/Employer";
        public const string Employer = "v1/Employer/{0}";
        public const string EmployersFiltered = "v1/Employer/Filtered?page={0}&pageSize={1}&nameFilter={2}&codeFilter={3}";

        public const string EmployerUsers = "v1/Employer/{0}/Users";
        public const string EmployerUser = "v1/User/{0}";

        public const string Settings = "v1/Employer/AllSettings/{0}";
    }
}