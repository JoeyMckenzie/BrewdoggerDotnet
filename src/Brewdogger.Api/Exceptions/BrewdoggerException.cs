using System;

namespace Brewdogger.Api.Exceptions
{
    public class BrewdoggerException : Exception
    {
        public BrewdoggerException()
            : base()
        {
        }

        public BrewdoggerException(string message)
            : base(message)
        {
        }
    }
}