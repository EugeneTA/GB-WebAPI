using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Models.Requests.Delete;
using MetricsAgent.Services;
using MetricsAgent.Services.Implemetation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    // api/metrics/dotnet/errors-count/from/{fromTime}/to/{toTime}

    [Route("api/metrics/dotnet/errors-count")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IDotNetMetricsRepository _dotNetMetricsRepository;

        #endregion

        public DotNetMetricsController(IDotNetMetricsRepository dotNetMetricsRepository, ILogger<DotNetMetricsController> logger)
        {
            _logger = logger;
            _dotNetMetricsRepository = dotNetMetricsRepository;
        }

        /// <summary>
        /// Добавление значения в репозиторий
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            _dotNetMetricsRepository.Create(new Models.DotNetMetric
            {
                Value = request.Value,
                Time = (int)request.Time.TotalSeconds
            });
            _logger.LogInformation("Create DotNet metrics add to repository call.");
            return Ok();
        }

        /// <summary>
        /// Удаление записи по id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("delete")]
        public IActionResult Delete([FromBody] DotNetMetricDeleteRequest request)
        {
            _dotNetMetricsRepository.Delete(request.Id);
            _logger.LogInformation("Delete DotNet metric call.");
            return Ok();
        }


        /// <summary>
        /// Получить статистику по ошибкам за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<DotNetMetric>> GetDotNetMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get DotNet metrics call.");
            return Ok(_dotNetMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }
    }
}
