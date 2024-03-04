using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployerUsersService
    {
        Task EmployUser(User user, Employer employer);
    }
}
