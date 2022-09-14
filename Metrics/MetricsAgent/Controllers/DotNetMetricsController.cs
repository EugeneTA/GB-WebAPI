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
    // api/metrics/dotnet/errors-count/from/{fromTime}/to/{toTime}

    [Route("api/metrics/dotnet/errors-count")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IDotNetMetricsRepository _dotNetMetricsRepository;
        private readonly IMapper _mapper;

        #endregion

        public DotNetMetricsController(
            IDotNetMetricsRepository dotNetMetricsRepository, 
            ILogger<DotNetMetricsController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dotNetMetricsRepository = dotNetMetricsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить статистику по ошибкам за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<DotNetMetricsResponse> GetDotNetMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get DotNet metrics call.");
            //return Ok(_dotNetMetricsRepository.GetByTimePeriod(fromTime, toTime));

            return Ok(new DotNetMetricsResponse()
            {
                Metrics = _dotNetMetricsRepository.GetByTimePeriod(fromTime, toTime)
                .Select(metric => _mapper.Map<DotNetMetricDto>(metric)).ToList()
            });
        }

        [HttpGet("all")]
        public ActionResult<DotNetMetricsResponse> GetAllMetrics()
        {
            _logger.LogInformation("Get all DotNet metrics call.");

            // Use Automapper
            return Ok(new DotNetMetricsResponse()
            {
                Metrics = _dotNetMetricsRepository.GetAll()
                .Select(metric => _mapper.Map<DotNetMetricDto>(metric)).ToList()
            });
        }

        ///// <summary>
        ///// Добавление значения в репозиторий
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost("create")]
        //public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        //{
        //    //_dotNetMetricsRepository.Create(new Models.DotNetMetric
        //    //{
        //    //    Value = request.Value,
        //    //    Time = (int)request.Time.TotalSeconds
        //    //});

        //    _dotNetMetricsRepository.Create(_mapper.Map<DotNetMetric>(request));

        //    _logger.LogInformation("Create DotNet metrics add to repository call.");
        //    return Ok();
        //}

        ///// <summary>
        ///// Удаление записи по id
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPut("delete")]
        //public IActionResult Delete([FromBody] DotNetMetricDeleteRequest request)
        //{
        //    _dotNetMetricsRepository.Delete(request.Id);
        //    _logger.LogInformation("Delete DotNet metric call.");
        //    return Ok();
        //}

    }
}
