using System.Net;

namespace CSI.IBTA.AuthService.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        public abstract HttpStatusCode StatusCode { get; }
        public string MessageHeader { get; }

        protected ExceptionBase(string message, string messageHeader = null!)
            : base(message)
        {
            MessageHeader = messageHeader;
        }
    }
}
