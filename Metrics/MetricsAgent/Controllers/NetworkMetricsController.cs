using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Models.Requests.Delete;
using MetricsAgent.Services;
using MetricsAgent.Services.Implemetation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    // api / metrics / network / from /{ fromTime}/ to /{ toTime}

    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly INetworkMetricsRepository _networkMetricsRepository;
        private readonly IMapper _mapper;

        #endregion

        public NetworkMetricsController(
            INetworkMetricsRepository networkMetricsRepository,
            ILogger<NetworkMetricsController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _networkMetricsRepository = networkMetricsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Добавление значения в репозиторий
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
        {
            //_networkMetricsRepository.Create(new Models.NetworkMetric
            //{
            //    Value = request.Value,
            //    Time = (int)request.Time.TotalSeconds
            //});

            _networkMetricsRepository.Create(_mapper.Map<NetworkMetric>(request));

            _logger.LogInformation("Create network metrics add to repository call.");
            return Ok();
        }

        /// <summary>
        /// Удаление записи по id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("delete")]
        public IActionResult Delete([FromBody] NetworkMetricDeleteRequest request)
        {
            _networkMetricsRepository.Delete(request.Id);
            _logger.LogInformation("Delete network metric call.");
            return Ok();
        }

        /// <summary>
        /// Получить статистику по нагрузке на сеть за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<NetworkMetricDto>> GetNetworkMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get network metrics call.");
            //return Ok(_networkMetricsRepository.GetByTimePeriod(fromTime,toTime));

            return Ok(_networkMetricsRepository.GetByTimePeriod(fromTime, toTime).
                Select(metric => _mapper.Map<NetworkMetricDto>(metric)).ToList());
        }
    }
}
