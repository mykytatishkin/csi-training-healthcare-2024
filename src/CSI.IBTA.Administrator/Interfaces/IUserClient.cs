using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Types;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IUserClient
    {
        Task<TaskResult> CreateEmployer(CreateEmployerDto dto);
    }
}
