using System.Text.Json.Serialization;
using MetricsManager.Models.Metrics;

namespace MetricsManager.Models.Requests.Response
{
    public class MetricsResponse<T> where T : class
    {
        public long AgentId { get; set; }

        [JsonPropertyName("metrics")]
        public T[] Metrics { get; set; }
    }
}
