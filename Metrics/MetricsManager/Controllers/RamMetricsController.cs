using MetricsManager.Models.Metrics;
using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Services;
using MetricsManager.Services.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly IRamMetricsAgentClient _metricAgentClient;
        private readonly IMetricAgentRepository _metricAgentRepository;

        public RamMetricsController(
            IRamMetricsAgentClient metricAgentClient,
            IMetricAgentRepository metricAgentRepository)
        {
            _metricAgentClient = metricAgentClient;
            _metricAgentRepository = metricAgentRepository;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<RamMetricsResponse> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            RamMetricsResponse ramMetricsResponse = _metricAgentClient.GetMetrics(new RamMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });

            return ramMetricsResponse == null ? BadRequest() : Ok(ramMetricsResponse);
        }

        [HttpGet("all/from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<RamMetricsResponse>> GetMetricsFromAll(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_metricAgentRepository.GetAll().Select(agent =>
            {
                var metrics = _metricAgentClient.GetMetrics(new RamMetricsRequest()
                {
                    AgentId = agent.AgentId,
                    FromTime = fromTime,
                    ToTime = toTime
                });

                return (metrics != null) ? metrics : new RamMetricsResponse() { AgentId = agent.AgentId, Metrics = new RamMetric[0] };
            }).ToList());
        }
    }
}
