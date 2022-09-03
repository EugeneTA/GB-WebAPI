using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
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

        public DotNetMetricsControllerTests()
        {
            _dotNetMetricsController = new DotNetMetricsController();
        }

        [Fact]
        public void GetDotNetMetrics_ReturnOk()
        {
            Assert.IsAssignableFrom<IActionResult>(_dotNetMetricsController.GetDotNetMetrics(new TimeSpan(10), new TimeSpan(100)));
        }
    }
}
