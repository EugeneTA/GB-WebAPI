using MetricsAgent.Options;
using MetricsAgent.Services;
using MetricsAgent.Services.Implemetation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Job
{
    public class CpuMetricJob : IJob
    {
        private PerformanceCounter _perfomanceCounter;
        private IServiceScopeFactory _serviceScopeFactory;
        private IOptions<CpuJobOptions> _options;

        public CpuMetricJob(IServiceScopeFactory serviceScopeFactory, IOptions<CpuJobOptions> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options;
            //_perfomanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            if (_options == null)
            {
                _perfomanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"); 
            }
            else
            {
                _perfomanceCounter = new PerformanceCounter(_options.Value.Name, _options.Value.Counter, _options.Value.Instance);
            }


            /*
            _perfomanceCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
            _perfomanceCounter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", "_Global_");
             */
        }


        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var cpuMetricsRepository = serviceScope.ServiceProvider.GetService<ICpuMetricsRepository>();
                try
                {
                    var cpuUsageInPercents = _perfomanceCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > CPU > {cpuUsageInPercents}");
                    cpuMetricsRepository.Create(new Models.CpuMetric
                    {
                        Value = (int)cpuUsageInPercents,
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
