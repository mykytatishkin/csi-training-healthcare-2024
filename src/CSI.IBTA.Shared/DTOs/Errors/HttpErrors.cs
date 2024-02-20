using System.Net;

namespace CSI.IBTA.Shared.DTOs.Errors
{
    public static class HttpErrors
    {
        public static HttpError InvalidCredentials = new("Invalid credentials", HttpStatusCode.Unauthorized);
    }
}
