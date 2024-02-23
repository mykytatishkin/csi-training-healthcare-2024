
namespace CSI.IBTA.Shared.DTOs.Errors
{
    public static class InternalErrors
    {
        public static InternalError BaseInternalError = new("Something went wrong");
        public static InternalError InvalidToken = new("Token is invalid");
    }
}
