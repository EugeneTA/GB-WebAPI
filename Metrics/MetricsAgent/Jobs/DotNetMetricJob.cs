using MetricsAgent.Options;
using MetricsAgent.Services;
using MetricsAgent.Services.Implemetation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Job
{
    public class DotNetMetricJob : IJob
    {
        private PerformanceCounter _perfomanceCounter;
        private IServiceScopeFactory _serviceScopeFactory;
        private IOptions<DotNetJobOptions> _options;

        public DotNetMetricJob(IServiceScopeFactory serviceScopeFactory, IOptions<DotNetJobOptions> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options;

            if (_options == null)
            {
                _perfomanceCounter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", "_Global_");
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
                var dotNetMetricsRepository = serviceScope.ServiceProvider.GetService<IDotNetMetricsRepository>();
                try
                {
                    var dotNetErrorCount = _perfomanceCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > DotNet > {dotNetErrorCount}");
                    dotNetMetricsRepository.Create(new Models.DotNetMetric
                    {
                        Value = (int)dotNetErrorCount,
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
