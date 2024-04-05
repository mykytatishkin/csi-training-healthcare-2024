namespace CSI.IBTA.Employer.Endpoints
{
    public static class BenefitsServiceEndpoints
    {
        public const string ImportContributions = "v1/Contributions";

        public const string Enrollments = "v1/Enrollments/{0}";
        public const string EnrollmentsByUserIds = "v1/Enrollments/GetByUserIds";
        public const string UpdateEnrollments = "v1/Enrollments/Employer/{0}/Employee/{1}";

        public const string InsurancePackagesByEmployer = "v1/InsurancePackage/GetFullByEmployer";

        public const string ActivePlansByNames = "v1/ActivePlansByNames?employerId={0}";
    }
}
