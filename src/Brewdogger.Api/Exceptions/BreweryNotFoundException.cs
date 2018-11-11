using System;

namespace Brewdogger.Api.Exceptions
{
    public class BreweryNotFoundException : Exception
    {
        public BreweryNotFoundException()
            : base()
        {
        }

        public BreweryNotFoundException(string message)
            : base(message)
        {
        }

        public BreweryNotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}