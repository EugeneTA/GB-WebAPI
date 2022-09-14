using MetricsManager.Models.Metrics;
using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Services;
using MetricsManager.Services.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly IDotNetMetricsAgentClient _metricAgentClient;
        private readonly IMetricAgentRepository _metricAgentRepository;

        public DotNetMetricsController(
            IDotNetMetricsAgentClient metricAgentClient,
            IMetricAgentRepository metricAgentRepository)
        {
            _metricAgentClient = metricAgentClient;
            _metricAgentRepository = metricAgentRepository;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<DotNetMetricsResponse> GetMetricsFromAgent(
                [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            DotNetMetricsResponse dotNetMetricsResponse = _metricAgentClient.GetMetrics(new DotNetMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });

            return dotNetMetricsResponse == null ? BadRequest() : Ok(dotNetMetricsResponse);
        }

        [HttpGet("all/from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<DotNetMetricsResponse>> GetMetricsFromAll(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_metricAgentRepository.GetAll().Select(agent =>
            {
                var metrics = _metricAgentClient.GetMetrics(new DotNetMetricsRequest()
                {
                    AgentId = agent.AgentId,
                    FromTime = fromTime,
                    ToTime = toTime
                });

                return (metrics != null) ? metrics : new DotNetMetricsResponse() { AgentId = agent.AgentId, Metrics = new DotNetMetric[0] };
            }).ToList());
        }
    }
}
