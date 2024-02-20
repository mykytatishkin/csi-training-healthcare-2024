namespace CSI.IBTA.Administrator.DTOs.EmployerUser
{
    public record CreateEmployerUserCommand(
        string Firstname,
        string Lastname,
        string Email,
        string Username,
        string Password,
        int EmployeeId);
}
