using System.Text.Json.Serialization;

namespace MetricsManager.Models.Metrics
{
    public class Metric
    {
        [JsonPropertyName("time")]
        public long Time { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
}

