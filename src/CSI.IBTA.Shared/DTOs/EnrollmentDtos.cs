namespace CSI.IBTA.Shared.DTOs
{
    public record EnrollmentDto(int Id, int PlanId, decimal Election, decimal Contribution, int EmployeeId);
    public record FullEnrollmentDto(int Id, PlanDto Plan, decimal Election, decimal Contribution, int EmployeeId);
    public record UpsertEnrollmentDto(int PlanId, decimal Election, int Id = 0);
    public record UpsertEnrollmentsDto(List<UpsertEnrollmentDto> Enrollments, byte[] EncodedEmployerEmployee);
    public record GetEnrollmentsDto(byte[] EncodedEmployerEmployee);
}
