using Dapper;
using MetricsManager.Models;
using MetricsManager.Options;
using Microsoft.Extensions.Options;
using System.Data.SQLite;

namespace MetricsManager.Services.Implementation
{
    public class MetricAgentRepository : IMetricAgentRepository
    {
        private ILogger<MetricAgentRepository> _logger;
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public MetricAgentRepository(IOptions<DatabaseOptions> databaseOptions, ILogger<MetricAgentRepository> logger)
        {
            _databaseOptions = databaseOptions;
            _logger = logger;
        }

        public bool Add(MetricsAgent value)
        {
            bool result = false;

            if (this.IsExist(value.AgentId))
            {
                result = this.Update(value);
            }
            else
            {
                // Using connection string from app settings file (appsettings.json)
                using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

                if (value != null)
                {
                    var executionResult = connection.Execute("INSERT INTO metricagents(agentid, agentaddress, enable) VALUES(@agentid, @agentaddress, @enable)", new
                    {
                        agentid = value.AgentId,
                        agentaddress = value.AgentAddress,
                        enable = value.Enable
                    });

                    result = executionResult > 0 ? true : false;
                }
            }

            return result;
        }

        public IList<MetricsAgent> GetAll()
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            //return connection.Query<AgentInfo>("SELECT * FROM metricagents").ToList();

            return connection.Query<MetricsAgent>("SELECT * FROM metricagents").ToList();

        }

        public MetricsAgent GetById(long id)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet

            return connection.QuerySingleOrDefault<MetricsAgent>("SELECT * FROM metricagents WHERE agentid=@agentid", new
            {
                agentid = id
            });
        }

        public bool IsExist(long id)
        {
            return this.GetById(id) == null ? false : true;
        }

        public bool Update(MetricsAgent value)
        {
            bool result = false;

            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            if (value != null)
            {
                var executionResult = connection.Execute("UPDATE metricagents SET agentaddress = @agentaddress, enable=@enable WHERE agentid = @agentid; ", new
                {
                    agentid = value.AgentId,
                    agentaddress = value.AgentAddress,
                    enable = value.Enable
                });

                result = executionResult > 0 ? true : false;
            }
        
            return result;
        }

        public bool Delete(long id)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            return connection.Execute("DELETE FROM metricagents WHERE agentid = @id; ", new
            {
                id = id
            }) > 0 ? true : false;

        }
    }
}
