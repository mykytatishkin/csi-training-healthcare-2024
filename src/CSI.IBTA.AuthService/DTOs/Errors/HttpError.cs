using System.Net;

namespace CSI.IBTA.AuthService.DTOs.Errors
{
    public record HttpError(string Title, HttpStatusCode StatusCode);
}
