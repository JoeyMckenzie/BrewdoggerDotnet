using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Exceptions;
using Brewdogger.Api.Models;
using Brewdogger.Api.Repositories;
using Serilog;

namespace Brewdogger.Api.Services
{
    public class BeerService : IBeerService
    {
        private readonly IMapper _mapper;
        private readonly IBeerRepository _beerRepository;
        
        public BeerService(IMapper mapper, IBeerRepository beerRepository)
        {
            _mapper = mapper;
            _beerRepository = beerRepository;
        }

        public ICollection<Beer> GetAllBeers()
        {
            var beers = _beerRepository.GetBeers();
            
            if (!beers.Any())
            {
                Log.Warning("BeerRepository::GetAllBeers - No beers found");
                throw new BeerNotFoundException("No beers found");
            }

            return beers;
        }
    }
}