using AutoMapper;
using MetricsManager.Models;
using System.Reflection;


namespace MetricsManager
{

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<MetricsAgent, AgentInfo>()
                .ForMember(x => x.AgentId, opt => opt.MapFrom(src => src.AgentId))
                .ForMember(x => x.AgentAddress, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.AgentAddress) ? null : new Uri(src.AgentAddress)))
                .ForMember(x => x.Enable, opt => opt.MapFrom(src => src.Enable));

            CreateMap<AgentInfo, MetricsAgent>()
                .ForMember(x => x.AgentId, opt => opt.MapFrom(src => src.AgentId))
                .ForMember(x => x.AgentAddress, opt => opt.MapFrom(src => src.AgentAddress.IsAbsoluteUri ? src.AgentAddress.ToString() : ""))
                .ForMember(x => x.Enable, opt => opt.MapFrom(src => src.Enable));
        }
    }

}
