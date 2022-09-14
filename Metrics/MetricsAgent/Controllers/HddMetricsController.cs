using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Models.Requests.Delete;
using MetricsAgent.Models.Requests.Response;
using MetricsAgent.Services;
using MetricsAgent.Services.Implemetation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    // api / metrics / hdd / left / from /{ fromTime}/ to /{ toTime}

    [Route("api/metrics/hdd/left")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<HddMetricsController> _logger;
        private readonly IHddMetricsRepository _hddMetricsRepository;
        private readonly IMapper _mapper;

        #endregion

        public HddMetricsController(
            IHddMetricsRepository hddMetricsRepository,
            ILogger<HddMetricsController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _hddMetricsRepository = hddMetricsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить статистику по нагрузке на HDD за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<HddMetricsResponse> GetHddMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get hdd metrics call.");
            //return Ok(_hddMetricsRepository.GetByTimePeriod(fromTime,toTime));

            return Ok(new HddMetricsResponse()
            {
                Metrics = _hddMetricsRepository.GetByTimePeriod(fromTime, toTime)
                .Select(metric => _mapper.Map<HddMetricDto>(metric)).ToList()
            });
        }

        [HttpGet("all")]
        public ActionResult<HddMetricsResponse> GetAllMetrics()
        {
            _logger.LogInformation("Get all Hdd metrics call.");

            // Use Automapper
            return Ok(new HddMetricsResponse()
            {
                Metrics = _hddMetricsRepository.GetAll()
                .Select(metric => _mapper.Map<HddMetricDto>(metric)).ToList()
            });
        }

        ///// <summary>
        ///// Добавление значения в репозиторий
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost("create")]
        //public IActionResult Create([FromBody] HddMetricCreateRequest request)
        //{
        //    //_hddMetricsRepository.Create(new Models.HddMetric
        //    //{
        //    //    Value = request.Value,
        //    //    Time = (int)request.Time.TotalSeconds
        //    //});

        //    _hddMetricsRepository.Create(_mapper.Map<HddMetric>(request));

        //    _logger.LogInformation("Create hdd metrics add to repository call.");
        //    return Ok();
        //}

        ///// <summary>
        ///// Удаление записи по id
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPut("delete")]
        //public IActionResult Delete([FromBody] HddMetricDeleteRequest request)
        //{
        //    _hddMetricsRepository.Delete(request.Id);
        //    _logger.LogInformation("Delete hdd metric call.");
        //    return Ok();
        //}


    }
}
