namespace MetricsManager.Models.Requests.Requests
{
    public class MetricsRequest
    {
        public long AgentId { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}
