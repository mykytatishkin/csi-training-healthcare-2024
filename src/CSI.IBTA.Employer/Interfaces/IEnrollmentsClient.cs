using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IInsuranceClient
    {
        Task<GenericResponse<List<EnrollmentDto>>> GetEmployeeEnrollments(int employeeId);
        Task<GenericResponse<List<PlanDto>>> GetEmployeePlans(int employeeId);
        Task<GenericResponse<List<FullInsurancePackageDto>>> GetEmployerPackages(int employerId);
        Task<GenericResponse<List<EnrollmentDto>>> UpdateEnrollments(int employerId, int employeeId, UpsertEnrollmentsDto updateEnrollmentsDto);
    }
}
