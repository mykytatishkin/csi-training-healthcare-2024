namespace CSI.IBTA.Shared.Authorization.Interfaces
{
    public interface IEmployeeOwnedResource
    {
        int EmployeeId { get; }
        int EmployerId { get; }
    }
}
