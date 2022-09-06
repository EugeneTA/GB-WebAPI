using MetricsAgent.Controllers;
using MetricsAgent.Services.Implemetation;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetricsAgent.Services;
using MetricsAgent.Models;
using Moq;
using MetricsAgent.Models.Dto;
using AutoMapper;
using MetricsAgent;
using Xunit.Priority;

namespace MetricsAgentTests
{
    // TODO: Домашнее задание [Пункт 3]
    //  Добавьте проект с тестами для агента сбора метрик. Напишите простые Unit-тесты на каждый
    // метод отдельно взятого контроллера в обоих тестовых проектах.

    public class CpuMetricsControllerTests
    {
        private CpuMetricsController _cpuMetricsController;
        private Mock<ILogger<CpuMetricsController>> _logger;
        private Mock<ICpuMetricsRepository> _repository;
        private Mock<IMapper> _mapper;

        public CpuMetricsControllerTests()
        {
            _logger = new Mock<ILogger<CpuMetricsController>>();
            _repository = new Mock<ICpuMetricsRepository>();
            _mapper = new Mock<IMapper>();
            //var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            //// Create mapper from our configuration
            //_mapper = mapperConfiguration.CreateMapper();

            _cpuMetricsController = new CpuMetricsController(_repository.Object, _logger.Object, _mapper.Object);
        }

        //[Fact]
        //public void GetCpuMetrics_ReturnOk()
        //{
        //    TimeSpan fromTime = TimeSpan.FromSeconds(0);
        //    TimeSpan toTime = TimeSpan.FromSeconds(100);
        //    var result = _cpuMetricsController.GetCpuMetrics(fromTime, toTime);
        //    Assert.IsAssignableFrom<IActionResult>(result);
        //}

        [Fact]
        [Priority(1)]
        public void CpuMetricsController_CreateTest()
        {
            _repository.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

            var result = _cpuMetricsController.Create(new MetricsAgent.Models.Requests.CpuMetricCreateRequest { Time = TimeSpan.FromSeconds(10), Value = 25 });

            _repository.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
        }

        [Fact]
        [Priority(2)]
        public void CpuMetricsController_GetTest()
        {
            _repository.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

            var result = _cpuMetricsController.Create(new MetricsAgent.Models.Requests.CpuMetricCreateRequest { Time = TimeSpan.FromSeconds(10), Value = 25 });

            Assert.IsAssignableFrom<ActionResult<IList<CpuMetricDto>>>(_cpuMetricsController.GetCpuMetrics(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1000)));
        }

        [Fact]
        [Priority(3)]
        public void CpuMetricsController_DeleteTest()
        {
            _repository.Setup(repository => repository.Delete(It.IsAny<int>())).Verifiable();

            var result = _cpuMetricsController.Delete(new MetricsAgent.Models.Requests.Delete.CpuMetricDeleteRequest { Id = 1 });

            _repository.Verify(repository => repository.Delete(It.IsAny<int>()), Times.AtMostOnce());
        }

    }


}
