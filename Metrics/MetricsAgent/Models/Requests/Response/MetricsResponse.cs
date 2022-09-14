using MetricsAgent.Models.Dto;

namespace MetricsAgent.Models.Requests.Response
{
    public class MetricsResponse<T> where T : class
    {
        public List<T>? Metrics { get; set; }
    }
}
