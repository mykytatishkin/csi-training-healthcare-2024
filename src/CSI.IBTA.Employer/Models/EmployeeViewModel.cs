using System.ComponentModel.DataAnnotations;

namespace CSI.IBTA.Employer.Models
{
    public class EmployeeViewModel
    {
        public string ActionName { get; set; } = null!;
        public int? UserId { get; set; }
        [Required]
        public int EmployerId { get; set; }
        [Required]
        public string Firstname { get; set; } = null!;
        [Required]
        public string Lastname { get; set; } = null!;
        [Required]
        public string SSN { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public string Street { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string State { get; set; } = null!;
        [Required]
        public string ZipCode { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
