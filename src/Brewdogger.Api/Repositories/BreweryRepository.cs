using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Exceptions;
using Brewdogger.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog;

namespace Brewdogger.Api.Repositories
{
    public class BreweryRepository : IBreweryRepository
    {
        private readonly BrewdoggerDbContext _context;

        public BreweryRepository(BrewdoggerDbContext context)
        {
            _context = context;
        }

        public Brewery GetBreweryById(int id)
        {
            var brewery = _context.Breweries.Find(id);

            return brewery;
        }

        public ICollection<Brewery> GetAllBreweries()
        {
            var breweries = _context.Breweries
                .Include(br => br.Beers)
                .ToList();

            return breweries;
        }
    }
}