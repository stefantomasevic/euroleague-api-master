using AutoMapper;
using Euroleague.DTO;
using Euroleague.Models;

namespace Euroleague.Mappings
{
    public class AutoMapperprofile:Profile
    {
        public AutoMapperprofile()
        {
            CreateMap<Player, PlayerDTO>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name));

            CreateMap<Team, TeamDTO>()
                .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players));

            CreateMap<TeamManipulationDTO, Team>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.Coach, opt => opt.MapFrom(src => src.Coach))
            .ForMember(dest => dest.Arena, opt => opt.MapFrom(src => src.Arena))
            .ForMember(dest => dest.Players, opt => opt.Ignore());


            CreateMap<EntryPlayerDto, Player>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.TeamId))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
             .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality))
            .ForMember(dest => dest.Team, opt => opt.Ignore());

            CreateMap<Team, TeamManipulationDTO>();


        }
    }
}
