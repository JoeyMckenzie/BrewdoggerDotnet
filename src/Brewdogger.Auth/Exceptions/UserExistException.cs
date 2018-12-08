using System;

namespace Brewdogger.Auth.Exceptions
{
    public class UserExistException : Exception
    {
        public UserExistException()
            : base()
        {
        }

        public UserExistException(string message)
            : base(message)
        {
        }

        public UserExistException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}