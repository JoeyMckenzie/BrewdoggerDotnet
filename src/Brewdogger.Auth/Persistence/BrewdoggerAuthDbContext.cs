using Brewdogger.Auth.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brewdogger.Auth.Persistence
{
    public sealed class BrewdoggerAuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public BrewdoggerAuthDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}