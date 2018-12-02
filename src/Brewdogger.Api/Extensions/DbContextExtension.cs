using Brewdogger.Api.Entities;
using Brewdogger.Api.Models;
using Brewdogger.Api.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Brewdogger.Api.Extensions
{
    public static class DbContextExtension
    {
        /// <summary>
        /// For development purposes, seed beers and breweries while
        /// instantiating a fresh context for each server restart
        /// </summary>
        /// <param name="context">Brewdogger database context</param>
        public static void Seed(this IApplicationBuilder app)
        {
            using (var context = app.ApplicationServices.GetRequiredService<BrewdoggerDbContext>())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Database.Migrate();

                AddBeers(context);
                AddBreweries(context);

                context.SaveChanges();
                context.Dispose();
            }
        }

        /// <summary>
        /// Method to seed beer entities to the context
        /// </summary>
        /// <param name="context">Brewdogger database context</param>
        private static void AddBeers(DbContext context)
        {
            // Seed beers
            var beer1 = new Beer()
            {
                BeerId = 1,
                Abv = 7.2,
                Ibu = 80,
                BeerName = "Hexagenia",
                BeerStyle = BeerStyle.Ipa,
                BreweryId = 1
            };      
            var beer2 = new Beer()
            {
                BeerId = 2,
                Abv = 9.2,
                Ibu = 120,
                BeerName = "Widowmaker",
                BeerStyle = BeerStyle.DoubleIpa,
                BreweryId = 1
            };     
            var beer3 = new Beer()
            {
                BeerId = 3,
                Abv = 5.5,
                Ibu = 75,
                BeerName = "Sierra Nevada Pale Ale",
                BeerStyle = BeerStyle.PaleAle,
                BreweryId = 2
            };
            
            // Seed entities
            context.AddRange(beer1, beer2, beer3);
        }

        /// <summary>
        /// Method to seed brewery entities to the context
        /// </summary>
        /// <param name="context">Brewdogger database context</param>
        private static void AddBreweries(DbContext context)
        {
            // Seed breweries
            var brewery1 = new Brewery()
            {
                BreweryId = 1,
                BreweryName = "Fall River Brewery",
                City = "Redding",
                State = "CA"
            };
            
            var brewery2 = new Brewery()
            {
                BreweryId = 2,
                BreweryName = "Sierra Nevada Brewing Company",
                City = "Chico",
                State = "CA"
            };
            
            // Seed entities
            context.AddRange(brewery1, brewery2);
        }
    }
}