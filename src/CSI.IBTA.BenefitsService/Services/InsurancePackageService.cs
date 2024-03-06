using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using AutoMapper;
using CSI.IBTA.Shared.DTOs.Errors;

namespace CSI.IBTA.BenefitsService.Services
{
    public class InsurancePackageService : IInsurancePackageService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;

        public InsurancePackageService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId)
        {
            var packages = await _benefitsUnitOfWork.Packages.Find(x => x.EmployerId == employerId && x.IsRemoved != true);

            return new GenericResponse<List<InsurancePackageDto>>(null, packages.Select(_mapper.Map<InsurancePackageDto>).ToList());
        }

        public async Task<GenericResponse<InsurancePackageDto>> InitializeInsurancePackage(int packageId)
        {
            var package = await _benefitsUnitOfWork.Packages.GetById(packageId);
            if(package == null) return new GenericResponse<InsurancePackageDto>(HttpErrors.ResourceNotFound, null);

            package.Initialized = DateOnly.FromDateTime(DateTime.UtcNow);
            _benefitsUnitOfWork.Packages.Upsert(package);
            await _benefitsUnitOfWork.CompleteAsync();

            return new GenericResponse<InsurancePackageDto>(null, _mapper.Map<InsurancePackageDto>(package));
        }

        public async Task<GenericResponse<bool>> RemoveInsurancePackage(int packageId)
        {
            var package = await _benefitsUnitOfWork.Packages.GetById(packageId);
            if (package == null) return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);

            package.IsRemoved = true;
            _benefitsUnitOfWork.Packages.Upsert(package);
            await _benefitsUnitOfWork.CompleteAsync();

            return new GenericResponse<bool>(null, true);
        }
    }
}
