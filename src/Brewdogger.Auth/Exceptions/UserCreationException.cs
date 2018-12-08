using System;

namespace Brewdogger.Auth.Exceptions
{
    public class UserCreationException : Exception
    {
        public UserCreationException()
            : base()
        {
        }

        public UserCreationException(string message)
            : base(message)
        {
        }

        public UserCreationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}