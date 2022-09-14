using MetricsManager.Models.Database;
using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Services;
using MetricsManager.Services.Client;
using Quartz;

namespace MetricsManager.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private IServiceScopeFactory _serviceScopeFactory;

        public NetworkMetricJob(
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var metricsRepository = serviceScope.ServiceProvider.GetService<INetworkMetricsRepository>();
                var metricAgentRepository = serviceScope.ServiceProvider.GetService<IMetricAgentRepository>();
                var agentClient = serviceScope.ServiceProvider.GetService<INetworkMetricsAgentClient>();

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

                                NetworkMetricsResponse metricsResponse = agentClient.GetMetrics(new NetworkMetricsRequest()
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
                                            metricsRepository.Create(new NetworkMetricDatabase
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
