using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Brewdogger.Api.Entities
{
    [Table("Breweries")]
    public class Brewery
    {
        public int BreweryId { get; set; }
        public string BreweryName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [ForeignKey("BreweryId")]
        public ICollection<Beer> Beers { get; set; }
    }
}