using CSI.IBTA.Shared.Authorization.Interfaces;

namespace CSI.IBTA.Shared.DTOs;

public record PagedEmployeesResponse(
    List<EmployeeDto> Employees,
    int CurrentPage,
    int PageSize,
    int TotalPages,
    int TotalCount);

public record EmployeeDto(
    int Id,
    string Firstname,
    string Lastname,
    string SSN,
    DateTime? DateOfBirth,
    int UserId);

public record CreateEmployeeDto(
    string UserName,
    string Password,
    string FirstName,
    string LastName,
    string SSN,
    string PhoneNumber,
    DateOnly DateOfBirth,
    string Email,
    string AddressState,
    string AddressStreet,
    string AddressCity,
    string AddressZip,
    int EmployerId);

public record FullEmployeeDto(
    int Id,
    string UserName,
    string Password,
    string FirstName,
    string LastName,
    string SSN,
    string PhoneNumber,
    DateOnly DateOfBirth,
    string Email,
    string AddressState,
    string AddressStreet,
    string AddressCity,
    string AddressZip,
    int EmployerId) : IEmployeeOwnedResource
{
    public int EmployeeId => Id;
}

public record UpdateEmployeeDto(
    int Id,
    string? Password,
    string FirstName,
    string LastName,
    string SSN,
    string PhoneNumber,
    DateOnly DateOfBirth,
    string Email,
    string AddressState,
    string AddressStreet,
    string AddressCity,
    string AddressZip);