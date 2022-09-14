using MetricsManager.Models.Metrics;
using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Services;
using MetricsManager.Services.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly IHddMetricsAgentClient _metricAgentClient;
        private readonly IMetricAgentRepository _metricAgentRepository;

        public HddMetricsController(
            IHddMetricsAgentClient metricAgentClient,
            IMetricAgentRepository metricAgentRepository)
        {
            _metricAgentClient = metricAgentClient;
            _metricAgentRepository = metricAgentRepository;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<HddMetricsResponse> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            HddMetricsResponse hddMetricsResponse = _metricAgentClient.GetMetrics(new HddMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });

            return hddMetricsResponse == null ? BadRequest() : Ok(hddMetricsResponse);
        }

        [HttpGet("all/from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<HddMetricsResponse>> GetMetricsFromAll(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_metricAgentRepository.GetAll().Select(agent =>
            {
                var metrics = _metricAgentClient.GetMetrics(new HddMetricsRequest()
                {
                    AgentId = agent.AgentId,
                    FromTime = fromTime,
                    ToTime = toTime
                });

                return (metrics != null) ? metrics : new HddMetricsResponse() { AgentId = agent.AgentId, Metrics = new HddMetric[0] };
            }).ToList());
        }

    }
}
