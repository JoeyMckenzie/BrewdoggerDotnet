using System.Collections.Generic;
using Brewdogger.Api.Entities;

namespace Brewdogger.Api.Services
{
    public interface IBreweryService
    {
        Brewery GetBrewery(int id);
        ICollection<Brewery> GetBreweries();
    }
}