using AutoMapper;
using Brewdogger.Api.Entities;
using Brewdogger.Api.Models;

namespace Brewdogger.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BeerDto, Beer>()
                .ForMember(b => b.BeerName, m => m.MapFrom(bd => bd.BeerName))
                .ForMember(b => b.BeerStyle, m => m.MapFrom(bd => bd.BeerStyle))
                .ForMember(b => b.Abv, m => m.MapFrom(bd => bd.Abv))
                .ForMember(b => b.Ibu, m => m.MapFrom(bd => bd.Ibu));

            CreateMap<Beer, BeerDto>()
                .ForMember(bd => bd.BeerName, m => m.MapFrom(b => b.BeerName))
                .ForMember(bd => bd.BeerStyle, m => m.MapFrom(b => b.BeerStyle))
                .ForMember(bd => bd.Abv, m => m.MapFrom(b => b.Abv))
                .ForMember(bd => bd.Ibu, m => m.MapFrom(b => b.Ibu));

            CreateMap<BreweryDto, Brewery>()
                .ForMember(b => b.BreweryName, m => m.MapFrom(bd => bd.BreweryName))
                .ForMember(b => b.City, m => m.MapFrom(bd => bd.City))
                .ForMember(b => b.State, m => m.MapFrom(bd => bd.State));

            CreateMap<Brewery, BreweryDto>()
                .ForMember(bd => bd.BreweryName, m => m.MapFrom(b => b.BreweryName))
                .ForMember(bd => bd.City, m => m.MapFrom(b => b.City))
                .ForMember(bd => bd.State, m => m.MapFrom(b => b.State));

            CreateMap<Beer, Beer>();
        }
    }
}