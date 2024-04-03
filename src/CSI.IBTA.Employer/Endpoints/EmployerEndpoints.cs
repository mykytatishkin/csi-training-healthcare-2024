namespace CSI.IBTA.Employer.Endpoints
{
    public static class EmployerEndpoints
    {
        public const string Employer = "v1/Employer";
        public const string GetEmployerSetting = "v1/Employer/settings/{0}?condition={1}";
        public const string UpdateEmployerClaimSetting = "v1/Employer/claimSetting/{0}";
        public const string GetEmployerByAccountId = "v1/Employer/GetByAccountId";
    }
}
