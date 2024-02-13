using System.Net;

namespace CSI.IBTA.AuthService.DTOs.Errors
{
    public static class Errors
    {
        public static HttpError InvalidCredentials = new("Invalid credentials", HttpStatusCode.Unauthorized);
    }
}
