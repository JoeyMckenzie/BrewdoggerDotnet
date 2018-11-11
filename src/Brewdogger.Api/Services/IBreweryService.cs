using System.Collections.Generic;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Models;

namespace Brewdogger.Api.Services
{
    public interface IBreweryService
    {
        Brewery GetBrewery(int id);
        void CreateBrewery(BreweryDto breweryDto);
        ICollection<Brewery> GetBreweries();
    }
}