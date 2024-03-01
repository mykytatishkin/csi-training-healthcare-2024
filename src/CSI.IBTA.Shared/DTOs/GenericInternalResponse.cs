using CSI.IBTA.Shared.DTOs.Errors;

namespace CSI.IBTA.Shared.DTOs
{
    public record GenericInternalResponse<T>(bool HasError, InternalError? Error, T? Result);
}
