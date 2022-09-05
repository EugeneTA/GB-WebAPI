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
    public class NetworkMetricsControllerTests
    {
        private NetworkMetricsController _networkMetricsController;
        private Mock<ILogger<NetworkMetricsController>> _logger;
        private Mock<INetworkMetricsRepository> _repository;

        public NetworkMetricsControllerTests()
        {
            _logger = new Mock<ILogger<NetworkMetricsController>>();
            _repository = new Mock<INetworkMetricsRepository>();
            _networkMetricsController = new NetworkMetricsController(_repository.Object, _logger.Object);
        }

        //[Fact]
        //public void GetNetworkMetrics_ResultOk()
        //{
        //    Assert.IsAssignableFrom<IActionResult>(_networkMetricsController.GetNetworkActions(new TimeSpan(1), new TimeSpan(200)));
        //}

        [Fact]
        public void NetworkMetricsController_CreateTest()
        {
            _repository.Setup(repository => repository.Create(It.IsAny<NetworkMetric>())).Verifiable();

            var result = _networkMetricsController.Create(new MetricsAgent.Models.Requests.NetworkMetricCreateRequest { Time = TimeSpan.FromSeconds(10), Value = 25 });

            _repository.Verify(repository => repository.Create(It.IsAny<NetworkMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void NetworkMetricsController_GetTest()
        {
            Assert.IsAssignableFrom<ActionResult<IList<NetworkMetric>>>(_networkMetricsController.GetNetworkMetrics(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(100)));
        }

        [Fact]
        public void NetworkMetricsController_DeleteTest()
        {
            _repository.Setup(repository => repository.Delete(It.IsAny<int>())).Verifiable();

            var result = _networkMetricsController.Delete(new MetricsAgent.Models.Requests.Delete.NetworkMetricDeleteRequest { Id = 1 });

            _repository.Verify(repository => repository.Delete(It.IsAny<int>()), Times.AtMostOnce());
        }
    }
}
