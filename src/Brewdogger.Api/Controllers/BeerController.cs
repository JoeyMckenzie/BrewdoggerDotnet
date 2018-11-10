using Brewdogger.Api.Repositories;
using Brewdogger.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Brewdogger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private readonly IBeerService _beerService;

        public BeerController(IBeerService beerService)
        {
            _beerService = beerService;
        }


        [HttpGet]
        public IActionResult GetAllBeers()
        {
            var beers = _beerService.GetAllBeers();

            return Ok(beers);
        }
    }
}