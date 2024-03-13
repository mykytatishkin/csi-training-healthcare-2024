
namespace CSI.IBTA.Shared.Entities
{
    public class EmployerUser
    {
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public Employer Employer { get; set; } = null!;
    }
}
