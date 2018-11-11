using System;
using System.Collections.Generic;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Repositories;
using System.Linq;
using AutoMapper;
using Brewdogger.Api.Exceptions;
using Brewdogger.Api.Mapping;
using Brewdogger.Api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Serilog;

namespace Brewdogger.Api.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly IBreweryRepository _breweryRepository;
        private readonly IMapper _mapper;

        public BreweryService(IBreweryRepository breweryRepository, IMapper mapper)
        {
            _breweryRepository = breweryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve a brewery by primary ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Brewery entity</returns>
        /// <exception cref="BreweryNotFoundException">Throws if no breweries found</exception>
        public Brewery GetBrewery(int id)
        {
            var brewery = _breweryRepository.GetBreweryById(id);
            
            if (brewery == null)
            {
                Log.Information("BreweryService::GetBreweryById - No brewery found with id [{0}]", id);
                throw new BreweryNotFoundException("No brewery found");
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
        /// <exception cref="BreweryNotFoundException"></exception>
        public ICollection<Brewery> GetBreweries()
        {
            var breweries = _breweryRepository.GetAllBreweries();

            if (!breweries.Any())
            {
                Log.Information("BreweryService::GetAllBreweries - No breweries found");
                throw new BreweryNotFoundException("No breweries found");
            }

            return breweries;
        }

        /// <summary>
        /// Create a brewery from a brewery DTO
        /// </summary>
        /// <param name="breweryDto">Brewery data transfer object</param>
        public void CreateBrewery(BreweryDto breweryDto)
        {
            // Map breweryDto to brewery object
            var newBrewery = _mapper.Map<BreweryDto, Brewery>(breweryDto);
            _breweryRepository.SaveBrewery(newBrewery);
            
            Log.Information("BreweryService::CreateBrewery - Brewery [{0}] saved successfully", newBrewery.BreweryName);
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