using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerTests
{
    public class RamMetricsControllerTests
    {
        private RamMetricsController _ramMetricsController;

        public RamMetricsControllerTests()
        {
            _ramMetricsController = new RamMetricsController();
        }

        [Fact]
        public void GetRamMetricsFromAgent_ResultOk()
        {
            Assert.IsAssignableFrom<IActionResult>(_ramMetricsController.GetMetricsFromAgent(4, new TimeSpan(1), new TimeSpan(200)));
        }

        [Fact]
        public void GetRamMetricsFromAll_ResultOK()
        {
            Assert.IsAssignableFrom<IActionResult>(_ramMetricsController.GetMetricsFromAll(new TimeSpan(1), new TimeSpan(200)));
        }
    }
}
