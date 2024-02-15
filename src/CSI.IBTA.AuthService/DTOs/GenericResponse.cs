using CSI.IBTA.AuthService.DTOs.Errors;

namespace CSI.IBTA.AuthService.DTOs
{
    public record GenericResponse<T>(bool HasError, HttpError? Error, T? Result);
}
