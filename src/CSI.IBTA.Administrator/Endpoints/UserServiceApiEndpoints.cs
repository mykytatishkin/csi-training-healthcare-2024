namespace CSI.IBTA.Administrator.Endpoints
{
    public class UserServiceApiEndpoints
    {
        public const string CreateUser = "v1/User";

        public const string GetEmployer = "v1/Employer/{0}";
        public const string GetEmployerUsers = "v1/Employer/{0}/Users";
        public const string PutEmployerUser = "v1/Employer/{0}/Users";
    }
}
