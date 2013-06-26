using System;

namespace RedBit.CCAdmin
{
    public class ResultArgs : EventArgs
    {
        public ResultArgs(Exception ex)
        {
            _error = ex;
        }

        private Exception _error;

        public Exception Error
        {
            get { return _error; }
            set { _error = value; }
        }

    }
}
