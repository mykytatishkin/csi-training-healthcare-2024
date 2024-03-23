namespace CSI.IBTA.Shared.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string? SSN { get; set; } // Employer User can be without SSN, thus - nullable.
        public DateTime? DateOfBirth { get; set; } // Employer User can be without Date of Birth, thus - nullable.
        public int AccountId { get; set; }
        public Employer? Employer { get; set; }
        public int? EmployerId { get; set; }
        public Account Account { get; set; } = null!;
        public IList<Address> Addresses { get; set; } = new List<Address>();
        public IList<Email> Emails { get; set; } = new List<Email>();
        public IList<Phone> Phones { get; set; } = new List<Phone>();
    }
}
