using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Shared.DTOs.EmployerUser
{
    public record CreateEmployerUserRequest(string UserName, string Password, string FirstName, string LastName, Role Role, int EmployerId, string PhoneNumber, string EmailAddress, string AddressState, string AddressStreet, string AddressCity, string AddressZip);
}
