using System;

namespace DebtsAPI.Services.Exceptions
{
    public class UserException : Exception
    {
        public UserException() : base() { }

        public UserException(string message) : base(message) { }

    }
}
