using AutoMapper;
using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
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
    public class RamMetricsControllerTests
    {
        private RamMetricsController _ramMetricsController;
        private Mock<ILogger<RamMetricsController>> _logger;
        private Mock<IRamMetricsRepository> _repository;
        private Mock<IMapper> _mapper;


        public RamMetricsControllerTests()
        {
            _logger = new Mock<ILogger<RamMetricsController>>();
            _repository = new Mock<IRamMetricsRepository>();
            _mapper = new Mock<IMapper>();
            _ramMetricsController = new RamMetricsController(_repository.Object, _logger.Object, _mapper.Object);
        }

        //[Fact]
        //public void GetRamMetrics_ResultOK()
        //{
        //    Assert.IsAssignableFrom<IActionResult>(_ramMerticsController.GetRamMetrics(new TimeSpan(1), new TimeSpan(20)));
        //}

        //[Fact]
        //public void RamMetricsController_CreateTest()
        //{
        //    _repository.Setup(repository => repository.Create(It.IsAny<RamMetric>())).Verifiable();

        //    var result = _ramMetricsController.Create(new MetricsAgent.Models.Requests.RamMetricCreateRequest { Time = TimeSpan.FromSeconds(10), Value = 25 });

        //    _repository.Verify(repository => repository.Create(It.IsAny<RamMetric>()), Times.AtMostOnce());
        //}

        [Fact]
        public void RamMetricsController_GetTest()
        {
            Assert.IsAssignableFrom<ActionResult<IList<RamMetricDto>>>(_ramMetricsController.GetRamMetrics(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(100)));
        }

        //[Fact]
        //public void RamMetricsController_DeleteTest()
        //{
        //    _repository.Setup(repository => repository.Delete(It.IsAny<int>())).Verifiable();

        //    var result = _ramMetricsController.Delete(new MetricsAgent.Models.Requests.Delete.RamMetricDeleteRequest { Id = 1 });

        //    _repository.Verify(repository => repository.Delete(It.IsAny<int>()), Times.AtMostOnce());
        //}
    }
}
