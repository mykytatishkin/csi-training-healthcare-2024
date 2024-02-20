using CSI.IBTA.Administrator.DTOs.EmployerUser;
using CSI.IBTA.Administrator.Models;
using AutoMapper;

namespace CSI.IBTA.Administrator.Mapping
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateEmployerUserViewModel, CreateEmployerUserCommand>();
        }
    }
}
