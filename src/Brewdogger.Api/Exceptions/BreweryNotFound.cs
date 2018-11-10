using System;

namespace Brewdogger.Api.Exceptions
{
    public class BreweryNotFound : Exception
    {
        public BreweryNotFound()
            : base()
        {
        }

        public BreweryNotFound(string message)
            : base(message)
        {
        }

        public BreweryNotFound(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}