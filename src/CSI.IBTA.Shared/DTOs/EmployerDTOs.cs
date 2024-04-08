using Microsoft.AspNetCore.Http;

namespace CSI.IBTA.Shared.DTOs
{
    public record EmployerDto(int Id, string Name, string Code, string Email, string Street, string City, string State, string ZipCode, string Phone, string? EncodedLogo);
    public record EmployerWithConsumerSettingDto(int Id, string Name, string Code, string Email, string Street, string City, string State, string ZipCode, string Phone, string? EncodedLogo, bool AllowAddConsumers);
    public record CreateEmployerDto(string Name, string Code, string Email, string Street, string City, string State, string ZipCode, string Phone, IFormFile? LogoFile);
    public record UpdateEmployerDto(string Name, string Code, string Email, string Street, string City, string State, string ZipCode, string Phone, IFormFile? NewLogoFile);
    public record SettingsDto(string Condition, bool State);
    public record UpdateClaimSettingDto(bool? EmployerState);
    public record SettingsWithEmployerStateDto(string Condition, bool State, bool? EmployerState);
    public record PagedEmployersResponse(
        List<EmployerDto> Employers,
        int CurrentPage,
        int PageSize,
        int TotalPages,
        int TotalCount);

    public record EmployerEmployeeDto(
        int employerId,
        int employeeId);

}
