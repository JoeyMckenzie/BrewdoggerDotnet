using System.Collections.Generic;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Models;

namespace Brewdogger.Api.Repositories
{
    public interface IBreweryRepository
    {
        Brewery GetBreweryById(int id);
        ICollection<Brewery> GetAllBreweries();
        void SaveBrewery(Brewery brewery);
    }
}