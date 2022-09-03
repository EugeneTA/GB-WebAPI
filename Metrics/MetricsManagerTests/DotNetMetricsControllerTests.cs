using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerTests
{
    public class DotNetMetricsControllerTests
    {
        private DotNetMetricsController _dotNetMetricsController;

        public DotNetMetricsControllerTests()
        {
            _dotNetMetricsController = new DotNetMetricsController(); ;
        }

        [Fact]
        public void GetDotNetMetricsFromAgent_ReturnOk()
        {
            int agentId = 1;
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);

            Assert.IsAssignableFrom<IActionResult>(_dotNetMetricsController.GetMetricsFromAgent(agentId, fromTime, toTime));
        }

        [Fact]
        public void GetDotNetMetricsAll_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);

            Assert.IsAssignableFrom<IActionResult>(_dotNetMetricsController.GetMetricsFromAll(fromTime, toTime));
        }
    }
}
