namespace MetricsAgent.Models.Requests
{
    public class MetricCreateRequestBase
    {
        public int Value { get; set; }

        public TimeSpan Time { get; set; }
    }
}
