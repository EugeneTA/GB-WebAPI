namespace MetricsManager.Models
{
    public class AgentInfo
    {
        public long AgentId { get; set; }
        public Uri AgentAddress { get; set; }
        public bool Enable { get; set; }
    }
}
