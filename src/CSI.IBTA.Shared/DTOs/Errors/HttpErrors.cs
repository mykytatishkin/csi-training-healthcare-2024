using System.Net;

namespace CSI.IBTA.Shared.DTOs.Errors
{
    public static class HttpErrors
    {
        public static HttpError InvalidCredentials = new("Invalid credentials", HttpStatusCode.Unauthorized);
        public static HttpError ResourceNotFound = new("Resource was not found", HttpStatusCode.NotFound);
        public static HttpError Conflict = new("Conflicting resource", HttpStatusCode.Conflict);
    }
}
