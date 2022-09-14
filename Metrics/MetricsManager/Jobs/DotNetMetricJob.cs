using MetricsManager.Models.Database;
using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Services;
using MetricsManager.Services.Client;
using Quartz;

namespace MetricsManager.Jobs
{
    public class DotNetMetricJob : IJob
    {
        private IServiceScopeFactory _serviceScopeFactory;

        public DotNetMetricJob(
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var metricsRepository = serviceScope.ServiceProvider.GetService<IDotNetMetricsRepository>();
                var metricAgentRepository = serviceScope.ServiceProvider.GetService<IMetricAgentRepository>();
                var agentClient = serviceScope.ServiceProvider.GetService<IDotNetMetricsAgentClient>();

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

                                DotNetMetricsResponse metricsResponse = agentClient.GetMetrics(new DotNetMetricsRequest()
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
                                            metricsRepository.Create(new DotNetMetricDatabase
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

