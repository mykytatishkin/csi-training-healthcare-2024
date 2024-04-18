namespace CSI.IBTA.Customer.Endpoints
{
    public static class BenefitsServiceEndpoints
    {
        public const string ClaimsByEmployee = "v1/Claims/ByEmployee/?page={0}&pageSize={1}&employeeId={2}";
        public const string ImportContributions = "v1/Contributions";

        public const string Enrollments = "v1/Enrollments/{0}";
        public const string ActivePagedEnrollments = "v1/Enrollments/{0}/ActivePaged?page={1}&pageSize={2}";
        public const string EnrollmentsByUserIds = "v1/Enrollments/GetByUserIds";
        public const string UpdateEnrollments = "v1/Enrollments/Employer/{0}/Employee/{1}";
        public const string GetEnrollmentsBalances = "v1/Enrollments/Balances";

        public const string InsurancePackagesByEmployer = "v1/InsurancePackage/GetFullByEmployer";

        public const string ActivePlansByNames = "v1/ActivePlansByNames?employerId={0}";

        public const string FileClaim = "v1/Claims/FileClaim";
    }
}
