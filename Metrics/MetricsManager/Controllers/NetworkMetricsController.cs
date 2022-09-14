using MetricsManager.Models.Metrics;
using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Services;
using MetricsManager.Services.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly INetworkMetricsAgentClient _metricAgentClient;
        private readonly IMetricAgentRepository _metricAgentRepository;

        public NetworkMetricsController(
            INetworkMetricsAgentClient metricAgentClient,
            IMetricAgentRepository metricAgentRepository)
        {
            _metricAgentClient = metricAgentClient;
            _metricAgentRepository = metricAgentRepository;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<NetworkMetricsResponse> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            NetworkMetricsResponse networkMetricsResponse = _metricAgentClient.GetMetrics(new NetworkMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });

            return networkMetricsResponse == null ? BadRequest() : Ok(networkMetricsResponse);
        }


        [HttpGet("all/from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<NetworkMetricsResponse>> GetMetricsFromAll(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_metricAgentRepository.GetAll().Select(agent =>
            {
                var metrics = _metricAgentClient.GetMetrics(new NetworkMetricsRequest()
                {
                    AgentId = agent.AgentId,
                    FromTime = fromTime,
                    ToTime = toTime
                });

                return (metrics != null) ? metrics : new NetworkMetricsResponse() { AgentId = agent.AgentId, Metrics = new NetworkMetric[0] };
            }).ToList());
        }
    }
}
