namespace CSI.IBTA.Shared.Entities
{
    public class Settings
    {
        public int Id { get; set; }
        public string Condition { get; set; } = null!;
        public bool State { get; set; }
        public int EmployerId { get; set; } 
        public Employer Employer { get; set; } = null!;
    }
}
