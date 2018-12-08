using AutoMapper;
using Brewdogger.Auth.Entities;
using Brewdogger.Auth.Models;

namespace Brewdogger.Auth.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>()
                .ForMember(u => u.FirstName, m => m.MapFrom(ud => ud.FirstName))
                .ForMember(u => u.LastName, m => m.MapFrom(ud => ud.LastName))
                .ForMember(u => u.Username, m => m.MapFrom(ud => ud.Username))
                .ForMember(u => u.Email, m => m.MapFrom(ud => ud.Email));
        }
    }
}