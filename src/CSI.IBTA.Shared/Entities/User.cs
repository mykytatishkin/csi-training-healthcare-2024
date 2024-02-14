namespace CSI.IBTA.Shared.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public Account Account { get; set; } = null!;
    }
}
