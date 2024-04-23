using System.ComponentModel.DataAnnotations;

namespace CSI.IBTA.Administrator.Models
{
    public class EmployerUserViewModel
    {
        public string ActionName { get; set; } = null!;
        public int? UserId { get; set; }
        public int EmployerId { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
