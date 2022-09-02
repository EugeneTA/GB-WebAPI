using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgentTests
{
    public class NetworkMetricsControllerTest
    {
        private NetworkMetricsController _networkMetricsController;

        public NetworkMetricsControllerTest()
        {
            _networkMetricsController = new NetworkMetricsController();
        }

        [Fact]
        public void GetNetworkMetrics_ResultOk()
        {
            Assert.IsAssignableFrom<IActionResult>(_networkMetricsController.GetNetworkActions(new TimeSpan(1), new TimeSpan(200)));
        }
    }
}
