using AutoMapper;
using MetricsAgent.Converters;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Models.Requests.Delete;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<CpuMetricsController> _logger;
        private readonly ICpuMetricsRepository _cpuMetricsRepository;
        private readonly IMapper _mapper;

        #endregion

        public CpuMetricsController(
            ICpuMetricsRepository cpuMetricsRepository,
            ILogger<CpuMetricsController> logger,
            IMapper mapper)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить статистику по нагрузке на ЦП за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<CpuMetricDto>> GetCpuMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get cpu metrics call.");
            //return Ok(_cpuMetricsRepository.GetByTimePeriod(fromTime, toTime));

            // Use Automapper
            return Ok(_cpuMetricsRepository.GetByTimePeriod(fromTime, toTime)
                .Select(metric => _mapper.Map<CpuMetricDto>(metric)).ToList());
        }

        [HttpGet("all")]
        public ActionResult<IList<CpuMetricDto>> GetAllMetrics()
        {
            _logger.LogInformation("Get all cpu metrics call.");
            //return Ok(_cpuMetricsRepository.GetByTimePeriod(fromTime, toTime));

            // Use Automapper
            return Ok(_cpuMetricsRepository.GetAll()
                .Select(metric => _mapper.Map<CpuMetricDto>(metric)).ToList());
        }


        ///// <summary>
        ///// Добавление значения в репозиторий
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost("create")]
        //public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        //{
        //    //_cpuMetricsRepository.Create(new Models.CpuMetric
        //    //{
        //    //    Value = request.Value,
        //    //    Time = (int)request.Time.TotalSeconds
        //    //});

        //    // Use Automapper
        //    _cpuMetricsRepository.Create(_mapper.Map<CpuMetric>(request));

        //    _logger.LogInformation("Create cpu metrics add to repository call.");
        //    return Ok();
        //}

        ///// <summary>
        ///// Удаление записи по id
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPut("delete")]
        //public IActionResult Delete([FromBody] CpuMetricDeleteRequest request)
        //{
        //    _cpuMetricsRepository.Delete(request.Id);
        //    _logger.LogInformation("Delete cpu metric call.");
        //    return Ok();
        //}

    }
}
