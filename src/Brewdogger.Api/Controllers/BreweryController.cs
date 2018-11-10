using Brewdogger.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Brewdogger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreweryController : ControllerBase
    {
        private readonly IBreweryService _breweryService;

        public BreweryController(IBreweryService breweryService)
        {
            _breweryService = breweryService;
        }

        [HttpGet]
        public IActionResult GetAllBreweries()
        {
            var breweries = _breweryService.GetBreweries();

            return Ok(breweries);
        }

        [HttpGet("{id}")]
        public IActionResult GetBreweryById(int id)
        {
            var brewery = _breweryService.GetBrewery(id);

            return Ok(brewery);
        }
    }
}