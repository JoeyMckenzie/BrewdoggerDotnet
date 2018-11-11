using System;

namespace Brewdogger.Api.Exceptions
{
    public class BeerNotFoundException : Exception
    {
        public BeerNotFoundException()
        {
        }

        public BeerNotFoundException(string message)
            : base(message)
        {
        }

        public BeerNotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}