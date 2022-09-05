using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgentTests
{
    public class HddMetricsControllerTests
    {
        private HddMetricsController _hddMetricsController;
        private Mock<ILogger<HddMetricsController>> _logger;
        private Mock<IHddMetricsRepository> _repository;

        public HddMetricsControllerTests()
        {
            _logger = new Mock<ILogger<HddMetricsController>>();
            _repository = new Mock<IHddMetricsRepository>();
            _hddMetricsController = new HddMetricsController(_repository.Object, _logger.Object);
        }

        //[Fact]
        //public void GetHDDMetrics_ResultOK()
        //{
        //    TimeSpan fromTime = TimeSpan.FromMilliseconds(1);
        //    TimeSpan toTime = TimeSpan.FromMilliseconds(200);
        //    Assert.IsAssignableFrom<IActionResult>(_hddMetricsController.GetHDDMetrics(fromTime, toTime));
        //}

        [Fact]
        public void HddMetricsController_CreateTest()
        {
            _repository.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();

            var result = _hddMetricsController.Create(new MetricsAgent.Models.Requests.HddMetricCreateRequest { Time = TimeSpan.FromSeconds(10), Value = 25 });

            _repository.Verify(repository => repository.Create(It.IsAny<HddMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void HddMetricsController_GetTest()
        {
            Assert.IsAssignableFrom<ActionResult<IList<HddMetric>>>(_hddMetricsController.GetHddMetrics(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(100)));
        }

        [Fact]
        public void HddMetricsController_DeleteTest()
        {
            _repository.Setup(repository => repository.Delete(It.IsAny<int>())).Verifiable();

            var result = _hddMetricsController.Delete(new MetricsAgent.Models.Requests.Delete.HddMetricDeleteRequest { Id = 1 });

            _repository.Verify(repository => repository.Delete(It.IsAny<int>()), Times.AtMostOnce());
        }
    }
}
