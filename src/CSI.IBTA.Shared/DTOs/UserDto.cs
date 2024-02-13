using CSI.IBTA.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.IBTA.Shared.DTOs
{
    public record UserDto(int Id, string UserName, string FirstName, string LastName, int AccoundId);
    public record NewUserDto(int Id, string UserName, string Password, string FirstName, string LastName, int AccoundId, Role Role);
    public record CreateUserDto(string UserName, string Password, string FirstName, string LastName, Role Role);
    public record UpdateUserDto(string UserName, string Password, string FirstName, string LastName);
}
