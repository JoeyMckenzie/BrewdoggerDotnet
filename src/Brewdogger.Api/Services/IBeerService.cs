using System.Collections.Generic;
using Brewdogger.Api.Entities;

namespace Brewdogger.Api.Services
{
    public interface IBeerService
    {
        ICollection<Beer> GetAllBeers();
    }
}