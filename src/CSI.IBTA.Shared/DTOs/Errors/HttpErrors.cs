using System.Net;

namespace CSI.IBTA.Shared.DTOs.Errors
{
    public static class HttpErrors
    {
        public static HttpError GenericError = new("Something went wrong...", HttpStatusCode.BadRequest);
        public static HttpError InvalidCredentials = new("Invalid credentials", HttpStatusCode.Unauthorized);
        public static HttpError ResourceNotFound = new("Resource was not found", HttpStatusCode.NotFound);
        public static HttpError Conflict = new("Conflicting resource", HttpStatusCode.Conflict);
        public static HttpError Forbidden = new("You do not have access to this resource", HttpStatusCode.Forbidden);
    }
}
