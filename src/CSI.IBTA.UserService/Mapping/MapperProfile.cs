using AutoMapper;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.UserService.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>()
                .ConstructUsing(user => new UserDto(
                    user.Id,
                    user.Account.Role,
                    user.Account.Username,
                    user.Firstname,
                    user.Lastname,
                    user.AccountId,
                    user.Employer != null ? user.Employer.Id : null,
                    user.Emails != null && user.Emails.Count > 0 ? user.Emails[0].EmailAddress : null,
                    user.Phones != null && user.Phones.Count > 0 ? user.Phones[0].PhoneNumber : null)
                    );

            CreateMap<User, NewUserDto>()
                .ConstructUsing(user => new NewUserDto(
                    user.Id,
                    user.Account.Username,
                    user.Account.Password,
                    user.Firstname,
                    user.Lastname,
                    user.Account.Id,
                    user.Employer == null ? null : user.Employer.Id,
                    user.Account.Role,
                    user.Phones[0].PhoneNumber,
                    user.Emails[0].EmailAddress,
                    user.Addresses[0].State,
                    user.Addresses[0].Street,
                    user.Addresses[0].City,
                    user.Addresses[0].Zip));

            CreateMap<User, UpdatedUserDto>()
                .ConstructUsing(user => new UpdatedUserDto(
                    user.Id,
                    user.Account.Username,
                    user.Account.Password,
                    user.Firstname,
                    user.Lastname,
                    user.Account.Id,
                    user.Account.Role,
                    user.Phones[0].PhoneNumber,
                    user.Emails[0].EmailAddress,
                    user.Addresses[0].State,
                    user.Addresses[0].Street,
                    user.Addresses[0].City,
                    user.Addresses[0].Zip));

            CreateMap<Employer, EmployerDto>()
                .ConstructUsing(employer => new EmployerDto(
                    employer.Id,
                    employer.Name,
                    employer.Code,
                    employer.Email,
                    employer.Street,
                    employer.City,
                    employer.State,
                    employer.Zip,
                    employer.Phone,
                    employer.Logo));

            CreateMap<User, EmployeeDto>();
        }
    }
}
