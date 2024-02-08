using System.Net;

namespace CSI.IBTA.AuthService.Exceptions
{
    public class InternalServerErrorException : ExceptionBase
    {
        private static string DefaultMessageHeader => "Internal Server Error";
        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

        public InternalServerErrorException(string message, string messageHeader = null!)
            : base(message, messageHeader ?? DefaultMessageHeader) { }
    }
}
