namespace Brewdogger.Api.Models
{
    public class BeerDto
    {
        public string BeerName { get; set; }
        public BeerStyle BeerStyle { get; set; }
        public float Abv { get; set; }
        public int Ibu { get; set; }
    }
}