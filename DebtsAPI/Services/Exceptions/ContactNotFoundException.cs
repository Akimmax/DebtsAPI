using System;

namespace DebtsAPI.Services.Exceptions
{
    public class ContactNotFoundException : Exception
    {
        public ContactNotFoundException(): base() { }

        public ContactNotFoundException(string message) : base(message) { }
    }
}
