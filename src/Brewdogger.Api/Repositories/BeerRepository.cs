using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Exceptions;
using Brewdogger.Api.Models;
using Brewdogger.Api.Persistence;
using Brewdogger.Api.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Brewdogger.Api.Repositories
{
    public class BeerRepository : IBeerRepository
    {
        private readonly BrewdoggerDbContext _context;
        private readonly BeerService _beerService;

        public BeerRepository(BrewdoggerDbContext context)
        {
            _context = context;
        }

        public async void CreateBeer(Beer beer)
        {
            await _context.Beers.AddAsync(beer);
            Log.Information("BeerRepository::CreateBeer - Beer with id [{0}] created successfully.", beer.BeerId);
        }

        public Beer GetBeerById(int id)
        {
            var beer = _context.Beers.Find(id);
            
            if (beer == null)
                throw new BeerNotFound("Could not find beer with id [" + id + "]");

            Log.Information("BeerRepository::GetBeerById - Beer with id [{0}] retrieved successfully.", beer.BeerId);
            return beer;
        }

        public ICollection<Beer> GetBeers()
        {
            var beers = _context.Beers
                .Include(br => br.Brewery)
                .ToList();
            
            return beers;
        }

        public void UpdateBeer(int id)
        {
            var beer = GetBeerById(id);
            
            
        }

        public void DeleteBeer(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}