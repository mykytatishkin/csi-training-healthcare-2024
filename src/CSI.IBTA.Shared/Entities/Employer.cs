namespace CSI.IBTA.Shared.Entities
{
    public class Employer
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Zip { get; set; } = null!;
        public string? Logo { get; set; } = null;
    }
}
