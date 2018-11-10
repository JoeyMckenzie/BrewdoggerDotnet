using System;

namespace Brewdogger.Api.Exceptions
{
    public class BeerNotFound : Exception
    {
        public BeerNotFound()
        {
        }

        public BeerNotFound(string message)
            : base(message)
        {
        }

        public BeerNotFound(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}