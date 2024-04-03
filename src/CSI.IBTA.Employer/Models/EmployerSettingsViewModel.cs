namespace CSI.IBTA.Employer.Models
{
    public class EmployerSettingsViewModel
    {
        public int EmployerId { get; set; }
        public bool AdminCondition { get; set; }
        public bool FollowAdminCondition { get; set; }
        public bool? EmployerAdminCondition { get; set; }
    }
}
