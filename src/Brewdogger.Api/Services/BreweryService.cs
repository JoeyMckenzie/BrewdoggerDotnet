using System;
using System.Collections.Generic;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Repositories;
using System.Linq;
using AutoMapper;
using Brewdogger.Api.Exceptions;
using Brewdogger.Api.Helpers;
using Brewdogger.Api.Mapping;
using Brewdogger.Api.Models;
using FluentValidation;
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
        /// Retrieves a brewery by primary ID.
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Brewery entity</returns>
        /// <exception cref="BreweryNotFoundException">Throws if no breweries found</exception>
        public Brewery GetBrewery(int id)
        {
            var brewery = _breweryRepository.FindBreweryById(id);
            
            if (brewery == null)
            {
                Log.Information("BreweryService::FindBreweryById - No brewery found with id [{0}]", id);
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
        /// Retrieves a list of all brewery entities in the context.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BreweryNotFoundException"></exception>
        public ICollection<Brewery> GetBreweries()
        {
            var breweries = _breweryRepository.FindAllBreweries();

            if (!breweries.Any())
            {
                Log.Information("BreweryService::FindAllBreweries - No breweries found");
                throw new BreweryNotFoundException("No breweries found");
            }

            return breweries;
        }

        /// <summary>
        /// Creates a brewery from a brewery DTO to begin tracking.
        /// </summary>
        /// <param name="breweryDto">Brewery data transfer object</param>
        public void CreateBrewery(BreweryDto breweryDto)
        {
            // Map breweryDto to brewery object
            var newBrewery = _mapper.Map<BreweryDto, Brewery>(breweryDto);
            
            if (ValidateBreweryExists(newBrewery))
                throw new BrewdoggerException("BreweryService::CreateBrewery - Brewery [" + newBrewery.BreweryName + "] already exists");
            
            // Validate brewery
            var breweryValidator = new BreweryValidator();
            var validatedBrewery = breweryValidator.Validate(newBrewery, ruleSet: "InputFields,StateRules");

            if (validatedBrewery.IsValid && validatedBrewery.Errors.Count == 0)
            {
                _breweryRepository.SaveBrewery(newBrewery);
                Log.Information("BreweryService::CreateBrewery - Brewery [{0}] saved successfully", newBrewery.BreweryName);
                return;
            }

            // Return validation error if any
            if (!validatedBrewery.IsValid)
            {
                Log.Error("BreweryService::CreateBrewery - Brewery is not valid, creation failed");
                
                if (validatedBrewery.Errors.Count > 0)
                {
                    foreach (var error in validatedBrewery.Errors)
                    {
                        Log.Information("BreweryService::CreateBrewery - Error code [{0}] validation error: {1}", error.ErrorCode, error.ErrorMessage);
                    }
                }
                
                throw new BrewdoggerException("Validation error while attempting to add brewery to database");
            }
        }

        /// <summary>
        /// Removes a brewery entity from the context.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="BreweryNotFoundException"></exception>
        public void DeleteBrewery(int id)
        {
            var brewery = _breweryRepository.FindBreweryById(id);

            if (brewery == null)
            {
                Log.Information("BreweryService::DeleteBrewery - Brewery with id [{0}] not found", id);
                throw new BreweryNotFoundException("Brewery not found");
            }
            
            _breweryRepository.DeleteBreweryById(brewery);
            Log.Information("BreweryService::DeleteBrewery - Brewery deleted");
        }

        /// <summary>
        /// Updates a brewery entity from the given DTO and id.
        /// </summary>
        /// <param name="id">Entity primary ID</param>
        /// <param name="breweryDto">Brewery entity</param>
        /// <exception cref="BreweryNotFoundException"></exception>
        public void UpdateBrewery(int id, BreweryDto breweryDto)
        {
            Brewery originalBrewery = null;
            var updatedBrewery = _mapper.Map<Brewery>(breweryDto);
            // Set the brewery id on the updatedBrewery before passing to validation
            updatedBrewery.BreweryId = id;
            
            try
            {
                originalBrewery = GetBrewery(id);
            }
            catch (BrewdoggerException brewdoggerException)
            {
                Log.Error("BreweryService::UpdateBrewery - No breweries found");
            }
            
            if (!ValidateBreweryExists(updatedBrewery))
                throw new BreweryNotFoundException("Brewery does not exist");

            if (originalBrewery != null)
            {
                _breweryRepository.UpdateBrewery(originalBrewery, updatedBrewery);
                Log.Information("BreweryService::UpdateBrewery - Brewery [{0}] successfully updated", id);
            }
        }

        /// <summary>
        /// Converts the brewery's state abbreviation to the full state name.
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
        
        /// <summary>
        /// Validate brewery does not exist before adding. Query existing breweries by id first,
        /// then by brewery name if no existing breweries by id are found.
        /// </summary>
        /// <param name="brewery">Brewery to check pre-existing status</param>
        /// <returns>Existence of brewery in database</returns>
        private bool ValidateBreweryExists(Brewery brewery)
        {
            var existingBreweries = _breweryRepository.FindBreweryById(brewery.BreweryId) ?? _breweryRepository.FindBreweryByBreweryName(brewery.BreweryName);

            return existingBreweries != null;
        }
    }
}