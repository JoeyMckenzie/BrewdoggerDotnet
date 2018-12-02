using Brewdogger.Api.Entities;
using Brewdogger.Api.Mapping;
using FluentValidation;

namespace Brewdogger.Api.Helpers
{
    public class BreweryValidator : AbstractValidator<Brewery>
    {
        public BreweryValidator()
        {
            RuleSet("InputFields", () =>
            {
                RuleFor(b => b.BreweryName).NotNull().NotEmpty();
                RuleFor(b => b.City).NotNull().NotEmpty();
                RuleFor(b => b.State).NotNull().NotEmpty();
            });      
            
            RuleSet("StateRules", () =>
            {
                RuleFor(b => b.State).Length(2);
                RuleFor(b => b.State).Must(ContainStateAbbreviationInDictionary);
            });
        }

        private static bool ContainStateAbbreviationInDictionary(string stateAbbreviation)
        {
            if (!string.IsNullOrWhiteSpace(stateAbbreviation))
                return StateMapping.StateMap.ContainsKey(stateAbbreviation);

            return false;
        }
    }
}