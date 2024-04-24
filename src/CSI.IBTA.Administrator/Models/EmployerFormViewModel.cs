using System.ComponentModel.DataAnnotations;

namespace CSI.IBTA.Administrator.Models
{
    public class EmployerFormViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? EncodedLogo { get; set; }
        public IFormFile? NewLogo { get; set; }
    }
}
