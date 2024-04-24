using System.ComponentModel.DataAnnotations;

namespace CSI.IBTA.Administrator.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
