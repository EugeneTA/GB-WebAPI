using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerTests
{
    public class NetworkMetricsControllerTests
    {
        private NetworkMetricsController _networkMetricsController;

        public NetworkMetricsControllerTests()
        {
            _networkMetricsController = new NetworkMetricsController();
        }

        [Fact]
        public void GetNetworkMetricsFromAgent_ResultOk()
        {
            Assert.IsAssignableFrom<IActionResult>(_networkMetricsController.GetMetricsFromAgent(3, new TimeSpan(1), new TimeSpan(200)));
        }

        [Fact]
        public void GetNetworkMetricsFromAll_ResultOK()
        {
            Assert.IsAssignableFrom<IActionResult>(_networkMetricsController.GetMetricsFromAll(new TimeSpan(1), new TimeSpan(200)));
        }
    }
}
