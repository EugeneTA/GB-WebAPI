using Lesson_01.Classes;
using Lesson_01.Controllers;
using Lesson_01.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit.Priority;

namespace Lesson_01_Tests
{
    public class WeatherForecastControllerTest
    {
        private WeatherForecastHolder _weatherForecastHolder;
        private WeatherForecastController _weatherForecastController;

        public WeatherForecastControllerTest()
        {
            _weatherForecastHolder = TestWeatherForecastHolder.Instance;
            _weatherForecastController = new WeatherForecastController(_weatherForecastHolder);
        }


        [Theory]
        [Priority(1)]
        [InlineData("2022.08.25", 32)]
        [InlineData("2022.07.22", 16)]
        [InlineData("2022.08.26", 28)]
        [InlineData("2022.08.27", -5)]
        public void AddForecatTest(DateTime dateTime, int temperatureC)
        {
            IActionResult actionResult = _weatherForecastController.Add(dateTime, temperatureC);
            Assert.IsAssignableFrom<IActionResult>(actionResult);
        }

        [Theory]
        [Priority(2)]
        [InlineData("2022.08.25", 5)]
        [InlineData("2022.07.22", 7)]
        [InlineData("2022.08.26", 22)]
        [InlineData("2022.08.27", 30)]
        public void UpdateForecatOkTest(DateTime dateTime, int temperatureC)
        {
            ActionResult<bool> actionResult = _weatherForecastController.Update(dateTime, temperatureC);
            Assert.True((bool)((OkObjectResult)actionResult.Result).Value);
        }

        [Theory]
        [Priority(3)]
        [InlineData("2022.08.21", 5)]

        public void UpdateForecatFaileTest(DateTime dateTime, int temperatureC)
        {
            ActionResult<bool> actionResult = _weatherForecastController.Update(dateTime, temperatureC);
            Assert.True((bool)((OkObjectResult)actionResult.Result).Value == false);
        }

        [Theory]
        [Priority(4)]
        [InlineData("2022.08.25", "2022.08.26")]
        [InlineData("2022.07.22", "2022.07.22")]
        public void DeleteForecastOkTest(DateTime fromDateTime, DateTime toDateTime)
        {
            ActionResult<bool> actionResult = _weatherForecastController.Delete(fromDateTime, toDateTime);
            Assert.True((bool)((OkObjectResult)actionResult.Result).Value);
        }

        [Theory]
        [Priority(5)]
        [InlineData("2021.05.25", "2021.05.28")]
        public void DeleteForecastFaileTest(DateTime fromDateTime, DateTime toDateTime)
        {
            ActionResult<bool> actionResult = _weatherForecastController.Delete(fromDateTime, toDateTime);
            Assert.True((bool)((OkObjectResult)actionResult.Result).Value == false);
        }

        [Theory]
        [Priority(6)]
        [InlineData("2022.08.25", "2022.08.27")]
        public void GetForecastDataOKTest(DateTime fromDateTime, DateTime toDateTime)
        {
            ActionResult<List<WeatherForecast>> actionResult = _weatherForecastController.Get(fromDateTime, toDateTime);
            Assert.NotNull(((OkObjectResult)actionResult.Result).Value);
            Assert.NotEmpty((List<WeatherForecast>)((OkObjectResult)actionResult.Result).Value);
        }

        [Theory]
        [Priority(7)]
        [InlineData("2021.08.25", "2021.08.27")]
        public void GetForecastDataFaileTest(DateTime fromDateTime, DateTime toDateTime)
        {
            ActionResult<List<WeatherForecast>> actionResult = _weatherForecastController.Get(fromDateTime, toDateTime);
            Assert.NotNull(((OkObjectResult)actionResult.Result).Value);
            Assert.Empty((List<WeatherForecast>)((OkObjectResult)actionResult.Result).Value);
        }


    }
}