using MetricsManager.Models.Database;
using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Services;
using MetricsManager.Services.Client;
using Quartz;

namespace MetricsManager.Jobs
{
    public class CpuMetricJob : IJob
    {
        private IServiceScopeFactory _serviceScopeFactory;

        public CpuMetricJob(
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var metricsRepository = serviceScope.ServiceProvider.GetService<ICpuMetricsRepository>();
                var metricAgentRepository = serviceScope.ServiceProvider.GetService<IMetricAgentRepository>();
                var agentClient = serviceScope.ServiceProvider.GetService<ICpuMetricsAgentClient>();
                
                try
                {
                    var agents = metricAgentRepository.GetAll().Where(agent => agent.Enable);

                    if (agents != null)
                    {
                        foreach (var agent in agents)
                        {
                            if (agent != null)
                            {
                                TimeSpan fromTime = metricsRepository.GetLastMetricTime(agent.AgentId);

                                CpuMetricsResponse metricsResponse = agentClient.GetMetrics(new CpuMetricsRequest()
                                {
                                    AgentId = agent.AgentId,
                                    FromTime = fromTime,
                                    ToTime = new TimeSpan(DateTime.UtcNow.ToLocalTime().Ticks)
                                });

                                if (metricsResponse != null)
                                {
                                    foreach (var metric in metricsResponse.Metrics)
                                    {
                                        if (metric != null)
                                        {
                                            metricsRepository.Create(new CpuMetricDatabase
                                            {
                                                AgentId = agent.AgentId,
                                                Value = metric.Value,
                                                Time = metric.Time
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }

            return Task.CompletedTask;
        }
    }
}
