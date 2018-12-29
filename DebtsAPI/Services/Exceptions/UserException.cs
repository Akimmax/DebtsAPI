using System;

namespace DebtsAPI.Services.Exeptions
{
    public class UserException : Exception
    {
        public UserException() : base() { }

        public UserException(string message) : base(message) { }

    }
}
