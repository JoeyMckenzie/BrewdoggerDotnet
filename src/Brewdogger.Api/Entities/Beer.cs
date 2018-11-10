using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Brewdogger.Api.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Brewdogger.Api.Entities
{
    [Table("Beers")]
    public class Beer
    {
        public int BeerId { get; set; }
        public string BeerName { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BeerStyle BeerStyle { get; set; }
        public double Abv { get; set; }
        public int Ibu { get; set; }
        public int BreweryId { get; set; }
        public Brewery Brewery { get; set; }
    }
}