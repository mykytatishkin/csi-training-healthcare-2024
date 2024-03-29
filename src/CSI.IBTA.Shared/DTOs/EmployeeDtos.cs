﻿namespace CSI.IBTA.Shared.DTOs;

public record PagedEmployeesResponse(
    List<EmployeeDto> Employees,
    int CurrentPage,
    int PageSize,
    int TotalPages,
    int TotalCount);

public record EmployeeDto(
    string Firstname,
    string Lastname,
    string SSN,
    DateTime DateOfBirth);
