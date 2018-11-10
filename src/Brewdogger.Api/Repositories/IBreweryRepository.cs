using System.Collections.Generic;
using Brewdogger.Api.Entities;

namespace Brewdogger.Api.Repositories
{
    public interface IBreweryRepository
    {
        Brewery GetBreweryById(int id);
        ICollection<Brewery> GetAllBreweries();
    }
}