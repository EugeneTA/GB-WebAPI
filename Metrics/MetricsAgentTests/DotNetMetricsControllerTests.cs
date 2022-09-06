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
    public class DotNetMetricsControllerTests
    {
        private DotNetMetricsController _dotNetMetricsController;
        private Mock<ILogger<DotNetMetricsController>> _logger;
        private Mock<IDotNetMetricsRepository> _repository;
        private Mock<IMapper> _mapper;

        public DotNetMetricsControllerTests()
        {
            _logger = new Mock<ILogger<DotNetMetricsController>>();
            _repository = new Mock<IDotNetMetricsRepository>();
            _mapper = new Mock<IMapper>();
            _dotNetMetricsController = new DotNetMetricsController(_repository.Object, _logger.Object, _mapper.Object);
        }

        //[Fact]
        //public void GetDotNetMetrics_ReturnOk()
        //{
        //    Assert.IsAssignableFrom<IActionResult>(_dotNetMetricsController.GetDotNetMetrics(new TimeSpan(10), new TimeSpan(100)));
        //}

        [Fact]
        public void DotNetMetricsController_CreateTest()
        {
            _repository.Setup(repository => repository.Create(It.IsAny<DotNetMetric>())).Verifiable();

            var result = _dotNetMetricsController.Create(new MetricsAgent.Models.Requests.DotNetMetricCreateRequest { Time = TimeSpan.FromSeconds(10), Value = 25 });

            _repository.Verify(repository => repository.Create(It.IsAny<DotNetMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void DotNetMetricsController_GetTest()
        {
            Assert.IsAssignableFrom<ActionResult<IList<DotNetMetricDto>>>(_dotNetMetricsController.GetDotNetMetrics(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(100)));
        }

        [Fact]
        public void DotNetMetricsController_DeleteTest()
        {
            _repository.Setup(repository => repository.Delete(It.IsAny<int>())).Verifiable();

            var result = _dotNetMetricsController.Delete(new MetricsAgent.Models.Requests.Delete.DotNetMetricDeleteRequest { Id = 1 });

            _repository.Verify(repository => repository.Delete(It.IsAny<int>()), Times.AtMostOnce());
        }
    }
}
