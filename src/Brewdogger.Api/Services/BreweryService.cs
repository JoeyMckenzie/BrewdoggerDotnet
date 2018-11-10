using System;
using System.Collections.Generic;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Repositories;
using System.Linq;
using Brewdogger.Api.Exceptions;
using Brewdogger.Api.Mapping;
using Serilog;

namespace Brewdogger.Api.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly IBreweryRepository _breweryRepository;

        public BreweryService(IBreweryRepository breweryRepository)
        {
            _breweryRepository = breweryRepository;
        }

        /// <summary>
        /// Retrieve a brewery by primary ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Brewery entity</returns>
        /// <exception cref="BreweryNotFound">Throws if no breweries found</exception>
        public Brewery GetBrewery(int id)
        {
            var brewery = _breweryRepository.GetBreweryById(id);
            
            if (brewery == null)
            {
                Log.Information("BreweryService::GetBreweryById - No brewery found with id [{0}]", id);
                throw new BreweryNotFound("No brewery found");
            }

            try
            {
                // Convert the state abbreviation to the full state name
                ConvertToFullStateName(brewery);
            }
            catch (ArgumentException argumentException)
            {
                // Just return the brewery if state abbreviation conversion fails
                return brewery;
            }

            return brewery;
        }

        /// <summary>
        /// Retrieve a list of all brewery entities in the database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BreweryNotFound"></exception>
        public ICollection<Brewery> GetBreweries()
        {
            var breweries = _breweryRepository.GetAllBreweries();

            if (!breweries.Any())
            {
                Log.Information("BreweryService::GetAllBreweries - No breweries found");
                throw new BreweryNotFound("No breweries found");
            }

            return breweries;
        }

        /// <summary>
        /// Convert the brewery's state abbreviation to the full state name
        /// </summary>
        /// <param name="brewery">Brewery entity</param>
        /// <exception cref="ArgumentException">State abbreviation not found, should be validated elsewhere</exception>
        private void ConvertToFullStateName(Brewery brewery)
        {
            var abbreviatedState = brewery.State;

            if (StateMapping.StateMap.ContainsKey(abbreviatedState))
                brewery.State = StateMapping.StateMap[abbreviatedState];
            else
            {
                Log.Information(
                    "BreweryService::BreweryService - State abbreviation not found [{0}] for BreweryId [{1}]",
                    abbreviatedState, brewery.BreweryId);
                throw new ArgumentException("State abbreviation not found [" + abbreviatedState + "]");
            }
        }
    }
}