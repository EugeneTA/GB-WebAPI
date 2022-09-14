using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;

namespace MetricsManager.Services.Client
{
    public interface IDotNetMetricsAgentClient : IMetricsAgentClient<DotNetMetricsResponse, DotNetMetricsRequest>
    {
    }
}
