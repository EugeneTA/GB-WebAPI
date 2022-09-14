using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;

namespace MetricsManager.Services.Client
{
    public interface ICpuMetricsAgentClient : IMetricsAgentClient<CpuMetricsResponse, CpuMetricsRequest>
    {
    }
    //public interface ICpuMetricsAgentClient
    //{
    //    CpuMetricsResponse GetMetrics(CpuMetricsRequest request);
    //}

}
