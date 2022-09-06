using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<DotNetMetric, DotNetMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<NetworkMetric, NetworkMetricDto>();
            CreateMap<RamMetric, RamMetricDto>();

            CreateMap<CpuMetricCreateRequest, CpuMetric>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (int)src.Time.TotalSeconds));
            CreateMap<DotNetMetricCreateRequest, DotNetMetric>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (int)src.Time.TotalSeconds));
            CreateMap<HddMetricCreateRequest, HddMetric>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (int)src.Time.TotalSeconds));
            CreateMap<NetworkMetricCreateRequest, NetworkMetric>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (int)src.Time.TotalSeconds));
            CreateMap<RamMetricCreateRequest, RamMetric>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (int)src.Time.TotalSeconds));
        }
    }
}
