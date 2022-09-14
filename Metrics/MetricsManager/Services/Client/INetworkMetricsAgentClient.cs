using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;

namespace MetricsManager.Services.Client
{
    public interface INetworkMetricsAgentClient : IMetricsAgentClient<NetworkMetricsResponse, NetworkMetricsRequest>
    {
    }
}
