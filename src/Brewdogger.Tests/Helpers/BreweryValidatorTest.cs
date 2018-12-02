using System.Collections.Generic;
using System.Linq;
using Brewdogger.Api.Helpers;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Brewdogger.Tests.Helpers
{
    [TestFixture]
    public class BreweryValidatorTest
    {
        private BreweryValidator _breweryValidator;
        private const string NullCode = "NotNullValidator";
        private const string EmptyCode = "NotEmptyValidator";

        [SetUp]
        public void Setup()
        {
            _breweryValidator = new BreweryValidator();
        }

        [Test]
        public void InputFields_ThrowsError_WhenBreweryNameIsNullOrEmpty()
        {
            // Act/Arrange
            var nullBreweryName = _breweryValidator.ShouldHaveValidationErrorFor(b => b.BreweryName, null as string, "InputFields");
            var emptyBreweryName = _breweryValidator.ShouldHaveValidationErrorFor(b => b.BreweryName, "", "InputFields");

            var nullErrorValidators = GetErrorCodes(nullBreweryName);
            var emptyErrorValidators = GetErrorCodes(emptyBreweryName);
            
            // Assert
            Assert.That(nullErrorValidators.Count(), Is.EqualTo(2));
            Assert.That(nullErrorValidators, Contains.Item(NullCode).And.Contains(EmptyCode));   
            Assert.That(emptyErrorValidators.Count(), Is.EqualTo(1));
            Assert.That(emptyErrorValidators, Contains.Item(EmptyCode));
        }
        
        [Test]
        public void InputFields_ThrowsError_WhenCityIsNullOrEmpty()
        {
            // Act/Arrange
            var nullCity = _breweryValidator.ShouldHaveValidationErrorFor(b => b.City, null as string, "InputFields");
            var emptyCity = _breweryValidator.ShouldHaveValidationErrorFor(b => b.City, "", "InputFields");

            var nullErrorValidators = GetErrorCodes(nullCity);
            var emptyErrorValidators = GetErrorCodes(emptyCity);
            
            // Assert
            Assert.That(nullErrorValidators.Count(), Is.EqualTo(2));
            Assert.That(nullErrorValidators, Contains.Item(NullCode).And.Contains(EmptyCode));   
            Assert.That(emptyErrorValidators.Count(), Is.EqualTo(1));
            Assert.That(emptyErrorValidators, Contains.Item(EmptyCode));
        }        
        
        [Test]
        public void InputFields_ThrowsError_WhenStateIsNullOrEmpty()
        {
            // Act/Arrange
            var nullState = _breweryValidator.ShouldHaveValidationErrorFor(b => b.State, null as string, "InputFields");
            var emptyState = _breweryValidator.ShouldHaveValidationErrorFor(b => b.State, "", "InputFields");

            var nullErrorValidators = GetErrorCodes(nullState);
            var emptyErrorValidators = GetErrorCodes(emptyState);
            
            // Assert
            Assert.That(nullErrorValidators.Count(), Is.EqualTo(2));
            Assert.That(nullErrorValidators, Contains.Item(NullCode).And.Contains(EmptyCode));   
            Assert.That(emptyErrorValidators.Count(), Is.EqualTo(1));
            Assert.That(emptyErrorValidators, Contains.Item(EmptyCode));
        }

        [Test]
        [TestCase("California")]
        [TestCase("WEST")]
        public void StateRules_ThrowsError_WhenStateHasLengthGreaterThanTwo(string stateAbbreviation)
        {
            var validationFailures = _breweryValidator.ShouldHaveValidationErrorFor(b => b.State, stateAbbreviation, "StateRules");
            var abbreviationLength = stateAbbreviation.Length;

            foreach (var validationError in validationFailures)
            {
                Assert.That(validationError.ErrorMessage, 
                    Is.EqualTo($"'State' must be 2 characters in length. You entered {abbreviationLength} characters.")
                        .Or.EqualTo("The specified condition was not met for 'State'."));
            }
        }

        /// <summary>
        /// Extracts the error code from the validation rules.
        /// </summary>
        /// <param name="validationFailures">Collection of validation failures</param>
        /// <returns></returns>
        private static IEnumerable<string> GetErrorCodes(IEnumerable<ValidationFailure> validationFailures)
        {
            var errorCodes = new List<string>();

            foreach (var validationFailure in validationFailures)
            {
                if (validationFailure.ErrorCode.Equals(NullCode))
                    errorCodes.Add(NullCode);
                
                if (validationFailure.ErrorCode.Equals(EmptyCode))
                    errorCodes.Add(EmptyCode);
            }

            return errorCodes;
        }
    }
}