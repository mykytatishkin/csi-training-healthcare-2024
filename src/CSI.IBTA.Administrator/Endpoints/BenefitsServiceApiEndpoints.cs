namespace CSI.IBTA.Administrator.Endpoints
{
    public static class BenefitsServiceApiEndpoints
    {
        public const string InsurancePackages = "v1/InsurancePackage";

        public const string Plan = "v1/InsurancePlans/{0}";
        public const string Plans = "v1/InsurancePlans";
        public const string PlanTypes = "v1/InsurancePlans/PlanTypes";
        public const string InsurancePackagesByEmployer = "v1/InsurancePackage/GetByEmployer";

        public const string Claims = "v1/Claims?page={0}&pageSize={1}&claimNumber={2}&employerId={3}&claimStatus={4}";
    }
}