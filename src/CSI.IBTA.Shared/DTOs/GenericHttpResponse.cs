using CSI.IBTA.Shared.DTOs.Errors;

namespace CSI.IBTA.Shared.DTOs
{
    public record GenericHttpResponse<T>(bool HasError, HttpError? Error, T? Result);
}
