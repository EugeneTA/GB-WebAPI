using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgentTests
{
    public class RamMetricsControllerTest
    {
        private RamMerticsController _ramMerticsController;

        public RamMetricsControllerTest()
        {
            _ramMerticsController = new RamMerticsController();
        }

        [Fact]
        public void GetRamMetrics_ResultOK()
        {
            Assert.IsAssignableFrom<IActionResult>(_ramMerticsController.GetRamMetrics(new TimeSpan(1), new TimeSpan(20)));
        }
    }
}
