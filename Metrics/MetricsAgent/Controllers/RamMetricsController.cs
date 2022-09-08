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
    // api / metrics / ram / available / from /{ fromTime}/ to /{ toTime}

    [Route("api/metrics/ram/available")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<RamMetricsController> _logger;
        private readonly IRamMetricsRepository _ramMetricsRepository;
        private readonly IMapper _mapper;

        #endregion

        public RamMetricsController(
            IRamMetricsRepository ramMetricsRepository,
            ILogger<RamMetricsController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _ramMetricsRepository = ramMetricsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить статистику по загрузке памяти за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<RamMetricDto>> GetRamMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get ram metrics call.");
            //return Ok(_ramMetricsRepository.GetByTimePeriod(fromTime, toTime));

            return Ok(_ramMetricsRepository.GetByTimePeriod(fromTime, toTime)
                .Select(metric => _mapper.Map<RamMetricDto>(metric)).ToList());
        }

        [HttpGet("all")]
        public ActionResult<IList<RamMetricDto>> GetAllMetrics()
        {
            _logger.LogInformation("Get all Ram metrics call.");

            // Use Automapper
            return Ok(_ramMetricsRepository.GetAll()
                .Select(metric => _mapper.Map<RamMetricDto>(metric)).ToList());
        }

        ///// <summary>
        ///// Добавление значения в репозиторий
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost("create")]
        //public IActionResult Create([FromBody] RamMetricCreateRequest request)
        //{
        //    //_ramMetricsRepository.Create(new Models.RamMetric
        //    //{
        //    //    Value = request.Value,
        //    //    Time = (int)request.Time.TotalSeconds
        //    //});

        //    _ramMetricsRepository.Create(_mapper.Map<RamMetric>(request));

        //    _logger.LogInformation("Create ram metrics add to repository call.");
        //    return Ok();
        //}

        ///// <summary>
        ///// Удаление записи по id
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPut("delete")]
        //public IActionResult Delete([FromBody] RamMetricDeleteRequest request)
        //{
        //    _ramMetricsRepository.Delete(request.Id);
        //    _logger.LogInformation("Delete ram metric call.");
        //    return Ok();
        //}


    }
}
