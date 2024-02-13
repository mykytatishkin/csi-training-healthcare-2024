namespace CSI.IBTA.Shared.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string State { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Zip { get; set; } = null!;
        public User Account { get; set; } = null!;
    }
}
