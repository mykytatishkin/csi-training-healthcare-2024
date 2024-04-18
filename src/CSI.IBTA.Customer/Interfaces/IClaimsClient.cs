using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Customer.Interfaces
{
    public interface IClaimsClient
    {
        Task<GenericResponse<bool>> FileClaim(FileClaimDto dto);
        Task<GenericResponse<PagedClaimsResponse>> GetClaimsByEmployee(int page, int pageSize, int employeeId);
    }
}
