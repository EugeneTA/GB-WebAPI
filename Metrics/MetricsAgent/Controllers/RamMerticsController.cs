using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    // api / metrics / ram / available / from /{ fromTime}/ to /{ toTime}

    [Route("api/metrics/ram/available")]
    [ApiController]
    public class RamMerticsController : ControllerBase
    {

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetRamMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
