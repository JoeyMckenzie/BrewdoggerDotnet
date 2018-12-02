using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Exceptions;
using Brewdogger.Api.Models;
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

        public Brewery FindBreweryById(int id)
        {
            var brewery = _context.Breweries.Find(id);
            return brewery;
        }

        public Brewery FindBreweryByBreweryName(string breweryName)
        {
            var brewery = _context.Breweries.FirstOrDefault(b => b.BreweryName == breweryName);
            return brewery;
        }

        public ICollection<Brewery> FindAllBreweries()
        {
            var breweries = _context.Breweries
                .Include(br => br.Beers)
                .ToList();

            return breweries;
        }

        public void SaveBrewery(Brewery brewery)
        {
            _context.Breweries.Add(brewery);
            _context.SaveChanges();
        }

        public void UpdateBrewery(Brewery originalBrewery, Brewery updatedBrewery)
        {
            _context.Entry(originalBrewery).CurrentValues.SetValues(updatedBrewery);
            _context.SaveChanges();
        }

        public void DeleteBreweryById(Brewery brewery)
        {
            _context.Breweries.Remove(brewery);
            _context.SaveChanges();
        }
    }
}