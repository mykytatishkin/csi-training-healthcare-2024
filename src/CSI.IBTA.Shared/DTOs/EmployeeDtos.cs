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
    DateTime? DateOfBirth);

public record CreateEmployeeDto(
    string UserName,
    string Password,
    string FirstName,
    string LastName,
    string SSN,
    string PhoneNumber,
    DateOnly DateOfBirth,
    string AddressState,
    string AddressStreet,
    string AddressCity,
    string AddressZip,
    int EmployerId);