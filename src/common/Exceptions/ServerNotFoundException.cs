using System;
using System.Net;

namespace RedBit.CCAdmin
{
    public class ServerNotFoundException : Exception
    {
        public ServerNotFoundException() { }
        public ServerNotFoundException(HttpStatusCode statusCode) : this(string.Format("Unable to connect to Server.  Status code: {0}", statusCode)) { }
        public ServerNotFoundException(WebExceptionStatus statusCode) : this(string.Format("Unable to connect to Server.  Web Exception Status code: {0}", statusCode)) { }
        public ServerNotFoundException(string message) : base(message) { }
        public ServerNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
