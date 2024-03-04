using AutoMapper;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.BenefitsService.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Package, InsurancePackageDto>()
                .ConstructUsing(x => new InsurancePackageDto(
                    x.Id,
                    x.Name,
                    x.Status,
                    x.Initialized == null,
                    x.Initialized != null)
                );
        }
    }
}