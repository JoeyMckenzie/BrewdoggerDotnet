using System;
using Brewdogger.Api.Exceptions;
using Brewdogger.Api.Models;
using Brewdogger.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

        /// <summary>
        /// GET all breweries /api/brewery
        /// </summary>
        /// <returns>Collection of all breweries</returns>
        [HttpGet]
        public IActionResult GetAllBreweries()
        {
            var breweries = _breweryService.GetBreweries();

            return Ok(breweries);
        }

        /// <summary>
        /// GET brewery /api/brewery/:id
        /// </summary>
        /// <param name="id">Brewery primary key</param>
        /// <returns>Requested brewery entity</returns>
        [HttpGet("{id}")]
        public IActionResult GetBreweryById(int id)
        {
            var brewery = _breweryService.GetBrewery(id);

            return Ok(brewery);
        }

        [HttpPost]
        public IActionResult CreateBrewery([FromBody] BreweryDto breweryDto)
        {
            try
            {
                _breweryService.CreateBrewery(breweryDto);
            }
            catch (BrewdoggerException be)
            {
                return BadRequest("Brewery could not be created");
            }

            return Created("Brewery created", breweryDto);
        }

        /// <summary>
        /// PUT brewery /api/brewery/:id
        /// </summary>
        /// <param name="id">Brewery primary key</param>
        /// <param name="breweryDto">Brewery DTO with values to update</param>
        /// <returns>Updated status code if successful</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateBreweryById(int id, [FromBody] BreweryDto breweryDto)
        {
            try
            {
                _breweryService.UpdateBrewery(id, breweryDto);
            }
            catch (BreweryNotFoundException breweryNotFoundException)
            {
                Log.Error("BreweryController::UpdateBrewery - Could not find any breweries to update");
                return BadRequest($"Could not update brewery with id [{id}]");
            }

            return Ok($"Brewery [{id}] updated successfully");
        }

        /// <summary>
        /// DELETE brewery /api/brewery/:id
        /// </summary>
        /// <param name="id">Brewery primary key</param>
        /// <returns>Ok status code if successful</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteBreweryById(int id)
        {
            try
            {
                _breweryService.DeleteBrewery(id);
            }
            catch (BreweryNotFoundException breweryNotFoundException)
            {
                Log.Error("BreweryController::DeleteBreweryById - Could not find any breweries to delete");
                return BadRequest($"Could not delete brewery with id [{id}]");
            }
            
            return Ok($"Brewery with id [{id}] deleted");
        }
    }
}