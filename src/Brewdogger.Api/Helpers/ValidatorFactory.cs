using System;
using System.Reflection;
using Brewdogger.Api.Entities;
using FluentValidation;

namespace Brewdogger.Api.Helpers
{
    public static class ValidatorFactor
    {
        public static IValidator GetValidator(Type validationType)
        {
            if (validationType.IsAssignableFrom(typeof(Beer)))
                return new BeerValidator();
            if (validationType.IsAssignableFrom(typeof(Brewery)))
                return new BreweryValidator();

            return null;
        }
    }
}