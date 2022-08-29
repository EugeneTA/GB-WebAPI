using Lesson_01.Classes;
using Lesson_01.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Lesson_01.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherForecastController : ControllerBase
    {

        /*
        HomeWork:

        �������� ���� ���������� � ������ � ���, ������� �� ������������� ��������� ����������������:
        � ����������� ��������� ����������� � ��������� �����
        � ����������� ��������������� ���������� ����������� � ��������� �����
        � ����������� ������� ���������� ����������� � ��������� ���������� �������
        � ����������� ��������� ������ ����������� ����������� �� ��������� ���������� �������

        */

        private readonly WeatherForecastHolder _weatherForecastHolder;

        #region Constructor
        public WeatherForecastController(WeatherForecastHolder weatherForecastHolder)
        {
            _weatherForecastHolder = weatherForecastHolder;
        }
        #endregion

        #region Methods

        [HttpPost("add")]
        public IActionResult Add([FromQuery] DateTime dateTime, [FromQuery] int temperatureC)
        {
            _weatherForecastHolder.Add(dateTime, temperatureC);
            return Ok();
        }

        [HttpPut("update")]
        public ActionResult<bool> Update([FromQuery] DateTime dateTime, [FromQuery] int temperatureC)
        {
            return Ok(_weatherForecastHolder.Update(dateTime, temperatureC));
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete([FromQuery] DateTime fromDateTime, [FromQuery] DateTime toDateTime)
        {            
            return Ok(_weatherForecastHolder.Delete(fromDateTime, toDateTime));
        }

        [HttpGet("get")]
        public ActionResult<List<WeatherForecast>> Get([FromQuery] DateTime fromDateTime, [FromQuery] DateTime toDateTime)
        {
            return Ok(_weatherForecastHolder.Get(fromDateTime, toDateTime));
        }

        #endregion



    }
}