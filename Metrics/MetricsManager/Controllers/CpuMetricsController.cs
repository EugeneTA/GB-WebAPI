using MetricsManager.Models.Metrics;
using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Services;
using MetricsManager.Services.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ICpuMetricsAgentClient _metricAgentClient;
        private readonly IMetricAgentRepository _metricAgentRepository;

        public CpuMetricsController(
            ICpuMetricsAgentClient metricAgentClient,
            IMetricAgentRepository metricAgentRepository)
        {
            _metricAgentClient = metricAgentClient;
            _metricAgentRepository = metricAgentRepository;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<CpuMetricsResponse> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            CpuMetricsResponse cpuMetricsResponse = _metricAgentClient.GetMetrics(new CpuMetricsRequest() 
            { 
                AgentId = agentId, 
                FromTime = fromTime, 
                ToTime = toTime
            });

            return cpuMetricsResponse == null ? BadRequest() : Ok(cpuMetricsResponse);
        }

        [HttpGet("all/from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<CpuMetricsResponse>> GetMetricsFromAll(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_metricAgentRepository.GetAll().Select(agent =>
                {
                    var metrics = _metricAgentClient.GetMetrics(new CpuMetricsRequest()
                    {
                        AgentId = agent.AgentId,
                        FromTime = fromTime,
                        ToTime = toTime
                    });

                    return (metrics != null) ? metrics : new CpuMetricsResponse() { AgentId = agent.AgentId, Metrics = new CpuMetric[0] };
                }).ToList());
        }
    }
}
