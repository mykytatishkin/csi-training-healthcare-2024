namespace CSI.IBTA.Shared.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Role Role { get; set; } = null!;
        public int RoleId { get; set; }
    }
}
