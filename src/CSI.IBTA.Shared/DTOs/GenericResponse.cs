using CSI.IBTA.Shared.DTOs.Errors;

namespace CSI.IBTA.Shared.DTOs
{
    public record GenericResponse<T>(HttpError? Error, T? Result);
}
