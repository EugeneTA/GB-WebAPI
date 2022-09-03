using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgentTests
{
    public class HddMetricsControllerTest
    {
        private HddMetricsController _hddMetricsController;
        public HddMetricsControllerTest()
        {
            _hddMetricsController = new HddMetricsController();
        }

        [Fact]
        public void GetHDDMetrics_ResultOK()
        {
            TimeSpan fromTime = TimeSpan.FromMilliseconds(1);
            TimeSpan toTime = TimeSpan.FromMilliseconds(200);
            Assert.IsAssignableFrom<IActionResult>(_hddMetricsController.GetHDDMetrics(fromTime, toTime));
        }
    }
}
