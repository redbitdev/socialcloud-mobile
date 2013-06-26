using System;

namespace RedBit.CCAdmin
{
    public class ServerErrorException : Exception
    {
		public ServerErrorException() { }
		public ServerErrorException(string message) : base(message) { }
		public ServerErrorException(string message, Exception inner) : base(message, inner) { }
    }
}
