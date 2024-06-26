﻿namespace CSI.IBTA.Employer.Endpoints
{
    public class UserServiceEndpoints
    {
        public const string UsersByUsernames = "v1/EmployeesByUsernames?employerId={0}";

        public const string Employees = "v1/Employee?page={0}&pageSize={1}&employerId={2}&firstname={3}&lastname={4}&ssn={5}";
        public const string EncryptedEmployee = "v1/Encrypt/Employer/{0}/Employee/{1}";
        public const string CreateEmployee = "v1/Employee";
        public const string GetEmployee = "v1/Employee/{0}";
        public const string UpdateEmployee = "v1/Employee/{0}";

        public const string Employer = "v1/Employer";
        public const string GetEmployerByAccountId = "v1/Employer/GetByAccountId";

        public const string GetEmployerSetting = "v1/Employer/settings/{0}?condition={1}";
        public const string UpdateEmployerClaimSetting = "v1/Employer/claimSetting/{0}";
    }
}