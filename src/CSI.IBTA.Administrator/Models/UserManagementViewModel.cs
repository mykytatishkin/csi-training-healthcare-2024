using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class UserManagementViewModel
    {
        public int EmployerId { get; set; }
        public List<UserDto>? EmployerUsers { get; set; }
    }
}