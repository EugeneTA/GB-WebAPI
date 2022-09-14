//using MetricsManager.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace MetricsManager.Controllers
//{
//    [Route("api/cpu")]
//    [ApiController]
//    public class CpuMetricsControllerOld : ControllerBase
//    {
//        private IMetricAgentRepository _metricAgentRepository;
//        public CpuMetricsControllerOld(
//            IMetricAgentRepository metricAgentRepository)
//        {
//            _metricAgentRepository = metricAgentRepository;
//        }

//        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
//        public IActionResult GetMetricsFromAgent(
//            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
//        {
//            return Ok();
//        }

//        [HttpGet("all/from/{fromTime}/to/{toTime}")]
//        public IActionResult GetMetricsFromAll(
//            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
//        {
//            return Ok();
//        }
//    }
//}
