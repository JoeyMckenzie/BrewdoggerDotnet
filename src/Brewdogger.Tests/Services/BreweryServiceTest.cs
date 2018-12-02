using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Helpers;
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
        private BreweryDto _breweryToUpdate;
        private Brewery _updatedBrewery;
        private Mock<BreweryValidator> _breweryValidator;

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

            _breweryToUpdate = new BreweryDto
            {
                BreweryName = "Sierra Nevada Brewing Company",
                City = "Mills River",
                State = "NC"
            };

            _updatedBrewery = new Brewery
            {
                BreweryId = 2,
                BreweryName = "Sierra Nevada Brewing Company",
                City = "Mills River",
                State = "NC"
            };

            _breweries = new Collection<Brewery> {_brewery1, _brewery2};
            
            // Mock repository
            _breweryRepository = new Mock<IBreweryRepository>();
            
            // Mock reads
            _breweryRepository.Setup(b => b.FindBreweryById(1)).Returns(_brewery1);
            _breweryRepository.Setup(b => b.FindBreweryById(2)).Returns(_brewery2);
            _breweryRepository.Setup(b => b.FindAllBreweries()).Returns(_breweries);
            
            // Mock creates
            _breweryRepository.Setup(b => b.SaveBrewery(It.IsAny<Brewery>())).Callback(() => _breweries.Add(_brewery2));
            _breweryRepository.Verify(b => b.SaveBrewery(It.IsAny<Brewery>()), Times.Once);
            
            // Mock deletes
            _breweryRepository.Setup(b => b.DeleteBreweryById(It.IsAny<Brewery>())).Callback(() => _breweries.Remove(_brewery2));
            
            // Mock updates
//            _breweryRepository.Setup(b => b.UpdateBrewery(_updatedBrewery))
//                .Callback(() => _brewery2 = _updatedBrewery);
            
            // Mock mapper
            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<BreweryDto, Brewery>(_breweryDto)).Returns(_brewery3);
            
            // Mock validator
            _breweryValidator = new Mock<BreweryValidator>();
            
//            _breweryService = new BreweryService(_breweryRepository.Object, _mapper.Object, _breweryValidator.Object);
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

        [Test]
        public void UpdateBrewery_WhenPassedValidUpdatedBrewery_UpdatesExistingEntityInDatabase()
        {
            // Arrange
            var originalBeer = _brewery2;

            // Act
            _breweryService.UpdateBrewery(2, _breweryToUpdate);

            // Assert
            
        }

        [Test]
        public void DeleteBrewery_WhenExistingId_DeletesBreweryEntityFromDatabase()
        {
            // Act
            _breweryService.DeleteBrewery(2);
            
            // Assert
            Assert.That(_breweries.Count, Is.EqualTo(1));
            Assert.That(_breweries.Contains(_brewery2), Is.False);
        }
    }
}