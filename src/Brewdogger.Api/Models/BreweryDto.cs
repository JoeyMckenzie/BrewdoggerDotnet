using System;

namespace Brewdogger.Api.Models
{
    public class BreweryDto
    {
        public int Id { get; set; }
        public string BreweryName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}