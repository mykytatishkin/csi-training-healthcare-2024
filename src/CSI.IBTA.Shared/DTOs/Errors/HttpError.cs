using System.Net;

namespace CSI.IBTA.Shared.DTOs.Errors
{
    public record HttpError(string Title, HttpStatusCode StatusCode);
}
