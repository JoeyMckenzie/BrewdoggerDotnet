using Brewdogger.Api.Entities;
using Brewdogger.Api.Repositories;
using Brewdogger.Api.Services;
using Moq;
using NUnit.Framework;

namespace Brewdogger.Tests.Services
{
    [TestFixture]
    public class BreweryServiceTest
    {
        private IBreweryService _breweryService;
        private Mock<IBreweryRepository> _breweryRepository;

        [SetUp]
        public void SetUp()
        {
            var brewery1 = new Brewery()
            {
                BreweryId = 1,
                BreweryName = "Fall River Brewery",
                City = "Redding",
                State = "CA"
            };
            
            var brewery2 = new Brewery()
            {
                BreweryId = 2,
                BreweryName = "Sierra Nevada Brewing Company",
                City = "Chico",
                State = "CA"
            };
            
            _breweryRepository = new Mock<IBreweryRepository>();
            _breweryRepository.Setup(b => b.GetBreweryById(1)).Returns(brewery1);
            _breweryRepository.Setup(b => b.GetBreweryById(2)).Returns(brewery2);
            
            _breweryService = new BreweryService(_breweryRepository.Object);
        }

        [Test]
        public void GetBreweryById_WhenBreweryIsFound_StateNameIsConverted()
        {
            // Arrange
            var brewery1 = _breweryService.GetBrewery(1);
            var brewery2 = _breweryService.GetBrewery(2);

            // Act
            var state1 = brewery1.State;
            var state2 = brewery2.State;

            // Assert
            Assert.That(state1, Is.EqualTo("California"));
            Assert.That(state2, Is.EqualTo("California"));
        }
    }
}