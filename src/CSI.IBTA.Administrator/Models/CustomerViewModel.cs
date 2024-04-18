using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class CustomerViewModel
    {
        public FullEmployeeDto Employee { get; set; }
        public CustomerViewModel(FullEmployeeDto fullEmployeeData) 
        {
            Employee = new FullEmployeeDto(
                fullEmployeeData.Id,
                fullEmployeeData.UserName,
                fullEmployeeData.Password,
                fullEmployeeData.FirstName,
                fullEmployeeData.LastName,
                new string('*', fullEmployeeData.SSN?.Length ?? 3) ?? "",
                fullEmployeeData.PhoneNumber,
                fullEmployeeData.DateOfBirth,
                fullEmployeeData.Email,
                fullEmployeeData.AddressState,
                fullEmployeeData.AddressStreet,
                fullEmployeeData.AddressCity,
                fullEmployeeData.AddressZip,
                fullEmployeeData.EmployerId);
        }
    }
}
