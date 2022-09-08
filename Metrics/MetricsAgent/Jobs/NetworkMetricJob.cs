using MetricsAgent.Options;
using MetricsAgent.Services;
using MetricsAgent.Services.Implemetation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace MetricsAgent.Job
{
    public class NetworkMetricJob : IJob
    {
        private PerformanceCounter _perfomanceCounter;
        private IServiceScopeFactory _serviceScopeFactory;
        private IOptions<NetworkJobOptions> _options;

        public NetworkMetricJob(IServiceScopeFactory serviceScopeFactory, IOptions<NetworkJobOptions> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options;

            //1121
            //Percentage of network bandwidth in use on this network segment.
            // "Intel[R] Wi-Fi 6 AX201 160MHz"
            // PerformanceCounter(тип параметра, название счетчика, сетевой интерфейс);

            //_perfomanceCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", "Intel[R] Wi-Fi 6 AX201 160MHz");


            if (_options == null)
            {
                PerformanceCounterCategory category = new PerformanceCounterCategory("Network Adapter");
                String[] instancename = category.GetInstanceNames();
                _perfomanceCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", instancename[0]);
            }
            else
            {
                _perfomanceCounter = new PerformanceCounter(_options.Value.Name, _options.Value.Counter, _options.Value.Instance);
            }


            //PerformanceCounterCategory category = new PerformanceCounterCategory("Network Adapter");
            //String[] instancename = category.GetInstanceNames();
            //PerformanceCounter[] performanceCounter =  category.GetCounters("Intel[R] Wi-Fi 6 AX201 160MHz");

            //foreach (string name in instancename)
            //{
            //    Debug.WriteLine($"Network instatse {name}");
            //}

            //foreach (var name in performanceCounter)
            //{
            //    Debug.WriteLine($"Network counters {name}");
            //}
        }


        public Task Execute(IJobExecutionContext context)
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var networkMetricsRepository = serviceScope.ServiceProvider.GetService<INetworkMetricsRepository>();
                try
                {
                    var networkUsageInPercents = _perfomanceCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > Network > {networkUsageInPercents}");
                    networkMetricsRepository.Create(new Models.NetworkMetric
                    {
                        Value = (int)networkUsageInPercents,
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
