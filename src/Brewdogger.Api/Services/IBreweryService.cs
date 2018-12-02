using System.Collections.Generic;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Models;

namespace Brewdogger.Api.Services
{
    public interface IBreweryService
    {
        Brewery GetBrewery(int id);
        ICollection<Brewery> GetBreweries();
        void CreateBrewery(BreweryDto breweryDto);
        void UpdateBrewery(int id, BreweryDto breweryDto);
        void DeleteBrewery(int id);
    }
}