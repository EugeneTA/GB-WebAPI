using MetricsAgent.Options;
using MetricsAgent.Services;
using MetricsAgent.Services.Implemetation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Job
{
    public class HddMetricJob : IJob
    {
        private PerformanceCounter _perfomanceCounter;
        private IServiceScopeFactory _serviceScopeFactory;
        private IOptions<HddJobOptions> _options;

        public HddMetricJob(IServiceScopeFactory serviceScopeFactory, IOptions<HddJobOptions> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options;

            if (_options == null)
            {
                _perfomanceCounter = new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");
            }
            else
            {
                _perfomanceCounter = new PerformanceCounter(_options.Value.Name, _options.Value.Counter, _options.Value.Instance);
            }   
        }


        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var hddMetricsRepository = serviceScope.ServiceProvider.GetService<IHddMetricsRepository>();
                try
                {
                    var hddUsageInPercents = _perfomanceCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > HDD > {hddUsageInPercents}");
                    hddMetricsRepository.Create(new Models.HddMetric
                    {
                        Value = (int)hddUsageInPercents,
                        Time = (long)time.TotalSeconds
                    });
                }
                catch (Exception ex)
                {

                }
            }


            return Task.CompletedTask;
        }
    }
}
