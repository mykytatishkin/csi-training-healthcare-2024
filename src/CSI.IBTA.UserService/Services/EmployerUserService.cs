using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;

namespace CSI.IBTA.UserService.Services
{
    public class EmployerUserService : IEmployerUsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployerUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task EmployUser(User user, Employer employer)
        {
            var employerUser = new EmployerUser
            {
                User = user,
                Employer = employer
            };

            await _unitOfWork.EmployerUsers.Add(employerUser);
            await _unitOfWork.CompleteAsync();
        }
    }
}
