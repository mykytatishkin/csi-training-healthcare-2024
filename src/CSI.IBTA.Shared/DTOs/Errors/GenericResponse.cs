namespace CSI.IBTA.Shared.DTOs.Errors
{
    public record GenericResponse<T>(HttpError? Error, T? Result);
}
