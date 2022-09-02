using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerTests
{
    public class HddMetricsControllerTests
    {
        private HddMetricsController _hddMetricsController;
        
        public HddMetricsControllerTests()
        {
            _hddMetricsController = new HddMetricsController();
        }

        [Fact]
        public void GetHDDMetricsFromAgent_ResultOk()
        {
            Assert.IsAssignableFrom<IActionResult>(_hddMetricsController.GetMetricsFromAgent(2, new TimeSpan(1), new TimeSpan(200)));
        }

        [Fact]
        public void GetHDDMetricsFromAll_ResultOK()
        {
            Assert.IsAssignableFrom<IActionResult>(_hddMetricsController.GetMetricsFromAll(new TimeSpan(1), new TimeSpan(200)));
        }
    }
}
