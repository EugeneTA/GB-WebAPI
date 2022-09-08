using MetricsAgent.Options;
using MetricsAgent.Services;
using MetricsAgent.Services.Implemetation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Job
{
    public class RamMetricJob : IJob
    {
        private PerformanceCounter _perfomanceCounter;
        private IServiceScopeFactory _serviceScopeFactory;
        private IOptions<RamJobOptions> _options;

        public RamMetricJob(IServiceScopeFactory serviceScopeFactory, IOptions<RamJobOptions> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options;

            if (_options == null)
            {
                _perfomanceCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
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
                var ramMetricsRepository = serviceScope.ServiceProvider.GetService<IRamMetricsRepository>();
                try
                {
                    var ramUsageInPercents = _perfomanceCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > Ram > {ramUsageInPercents}");
                    ramMetricsRepository.Create(new Models.RamMetric
                    {
                        Value = (int)ramUsageInPercents,
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
