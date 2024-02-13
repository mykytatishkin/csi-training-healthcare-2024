using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Shared.DTOs
{
    public record UserDto(int Id, string UserName, string FirstName, string LastName, int AccoundId);
    public record NewUserDto(int Id, string UserName, string Password, string FirstName, string LastName, int AccoundId, Role Role, string PhoneNumber, string EmailAddress, string AddressState, string AddressStreet, string AddressCity, string AddressZip);
    public record CreateUserDto(string UserName, string Password, string FirstName, string LastName, Role Role, string PhoneNumber, string EmailAddress, string AddressState, string AddressStreet, string AddressCity, string AddressZip);
    public record UpdateUserDto(string UserName, string Password, string FirstName, string LastName, string PhoneNumber, string EmailAddress, string AddressState, string AddressStreet, string AddressCity, string AddressZip);
}
