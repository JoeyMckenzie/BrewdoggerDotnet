using System;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Brewdogger.Api.Persistence
{
    public sealed class BrewdoggerDbContext : DbContext
    {
        public DbSet<Beer> Beers { get; set; }
        public DbSet<Brewery> Breweries { get; set; }

        public BrewdoggerDbContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
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

            // Convert BeerStyle enum to string
            modelBuilder
                .Entity<Beer>()
                .Property(b => b.BeerStyle)
                .HasConversion(new EnumToStringConverter<BeerStyle>());

            // Configure many-to-one relationship for Beers table 
            modelBuilder
                .Entity<Beer>()
                .HasOne(b => b.Brewery)
                .WithMany(b => b.Beers)
                .HasForeignKey(fk => fk.BreweryId);

            // Configure one-to-many relationship for Breweries table
            modelBuilder
                .Entity<Brewery>()
                .HasMany(b => b.Beers)
                .WithOne(b => b.Brewery);
            
            // Create index for State
            modelBuilder
                .Entity<Brewery>()
                .HasIndex(b => b.State);

            // Seed data
            modelBuilder
                .Entity<Beer>()
                .HasData(beer1, beer2, beer3);

            modelBuilder
                .Entity<Brewery>()
                .HasData(brewery1, brewery2);
        }
    }
}