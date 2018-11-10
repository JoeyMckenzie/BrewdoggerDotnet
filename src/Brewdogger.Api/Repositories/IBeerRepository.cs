using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Models;

namespace Brewdogger.Api.Repositories
{
    public interface IBeerRepository
    {
        void CreateBeer(Beer beer);
        Beer GetBeerById(int id);
        ICollection<Beer> GetBeers();
        void UpdateBeer(int id);
        void DeleteBeer(int id);
    }
}