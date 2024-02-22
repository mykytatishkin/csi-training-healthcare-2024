using System.ComponentModel.DataAnnotations;

namespace CSI.IBTA.Administrator.Models
{
    public class EmployerUserViewModel
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
        public string Email { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; } = null!;
    }
}
