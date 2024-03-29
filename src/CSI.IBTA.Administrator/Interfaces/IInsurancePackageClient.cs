﻿using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IInsurancePackageClient
    {
        Task<GenericResponse<bool?>> CreateInsurancePackage(CreateInsurancePackageDto command);
        Task<GenericResponse<bool?>> UpdateInsurancePackage(UpdateInsurancePackageDto command);
        Task<GenericResponse<FullInsurancePackageDto>> GetInsurancePackage(int insurancePackageId);
        Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId);
        Task<GenericResponse<InsurancePackageDto>> InitializeInsurancePackage(int packageId);
        Task<GenericResponse<bool>> RemoveInsurancePackage(int packageId);
    }
}