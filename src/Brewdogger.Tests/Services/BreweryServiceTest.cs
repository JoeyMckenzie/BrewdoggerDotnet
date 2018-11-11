using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Models;
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
        private Mock<IMapper> _mapper;
        private Brewery _brewery1;
        private Brewery _brewery2;
        private Brewery _brewery3;
        private ICollection<Brewery> _breweries;
        private BreweryDto _breweryDto;

        [SetUp]
        public void SetUp()
        {
            _brewery1 = new Brewery
            {
                BreweryId = 1,
                BreweryName = "Fall River Brewery",
                City = "Redding",
                State = "CA"
            };
            
            _brewery2 = new Brewery
            {
                BreweryId = 2,
                BreweryName = "Sierra Nevada Brewing Company",
                City = "Chico",
                State = "CA"
            };

            _brewery3 = new Brewery
            {
                BreweryId = 3,
                BreweryName = "Coors Brewing Company",
                City = "Golden",
                State = "CO"
            };

            _breweryDto = new BreweryDto
            {
                BreweryName = "Coors Brew Company",
                City = "Golden",
                State = "CO"
            };

            _breweries = new Collection<Brewery> {_brewery1, _brewery2};
            
            // Mock repository
            _breweryRepository = new Mock<IBreweryRepository>();
            _breweryRepository.Setup(b => b.GetBreweryById(1)).Returns(_brewery1);
            _breweryRepository.Setup(b => b.GetBreweryById(2)).Returns(_brewery2);
            _breweryRepository.Setup(b => b.GetAllBreweries()).Returns(_breweries);
            _breweryRepository.Setup(b => b.SaveBrewery(_brewery3)).Callback(() => _breweries.Add(_brewery3));
            
            // Mock mapper
            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<BreweryDto, Brewery>(_breweryDto)).Returns(_brewery3);
            
            _breweryService = new BreweryService(_breweryRepository.Object, _mapper.Object);
        }

        [Test]
        public void GetAllBreweries_WhenBreweriesExist_ReturnsListOfBreweries()
        {
            // Act
            var breweries = _breweryService.GetBreweries();

            // Assert
            Assert.That(breweries.Count, Is.EqualTo(2));
            Assert.That(breweries, Contains.Item(_brewery1).And.Contains(_brewery2));
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

        [Test]
        public void CreateBrewery_WhenPassedValidBreweryDTO_CreatesBreweryEntityInDatabase()
        {
            // Act
            _breweryService.CreateBrewery(_breweryDto);

            // Assert
            Assert.That(_breweries.Count, Is.EqualTo(3));
            Assert.That(_breweries.Contains(_brewery3));
        }
    }
}