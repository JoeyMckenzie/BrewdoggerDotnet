using System.Collections.Generic;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Models;

namespace Brewdogger.Api.Repositories
{
    public interface IBreweryRepository
    {
        Brewery FindBreweryById(int id);
        Brewery FindBreweryByBreweryName(string breweryName);
        ICollection<Brewery> FindAllBreweries();
        void SaveBrewery(Brewery brewery);
        void UpdateBrewery(Brewery brewery, Brewery updatedBrewery);
        void DeleteBreweryById(Brewery brewery);
    }
}