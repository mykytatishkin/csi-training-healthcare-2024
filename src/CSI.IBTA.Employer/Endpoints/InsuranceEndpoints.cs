namespace CSI.IBTA.Employer.Endpoints
{
    public static class InsuranceEndpoints
    {
        public const string Enrollments = "v1/Enrollments/{0}";
        public const string UpdateEnrollments = "v1/Enrollments/Employer/{0}/Employee/{1}";

        public const string InsurancePackagesByEmployer = "v1/InsurancePackage/GetFullByEmployer";
    }
}