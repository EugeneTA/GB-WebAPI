using MetricsManager.Models.Requests.Response;

namespace MetricsManager.Services.Client
{
    public interface IMetricsAgentClient<Response, Request> where Response: class where Request : class
    {
        Response GetMetrics(Request request);
        Response GetAllMetrics(Request request);
    }
}
