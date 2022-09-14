using Dapper;
using MetricsManager.Models;
using MetricsManager.Models.Database;
using MetricsManager.Options;
using Microsoft.Extensions.Options;
using System.Data.SQLite;

namespace MetricsManager.Services.Implementation
{

    public class HddMetricsRepository : IHddMetricsRepository
    {

        #region Services

        private readonly IOptions<DatabaseOptions> _databaseOptions;

        #endregion

        public HddMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }


        public void Create(HddMetricDatabase item)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            connection.Execute("INSERT OR IGNORE INTO hddmetrics(agentid, value, time) VALUES(@agentid, @value, @time)", new
            {
                agentid = item.AgentId,
                value = item.Value,
                time = item.Time
            });

        }

        public void Delete(long id)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            connection.Execute("DELETE FROM hddmetrics WHERE agentid=@agentid", new
            {
                agentid = id
            });

        }

        public void Update(HddMetricDatabase item)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            connection.Execute("UPDATE hddmetrics SET value = @value, time = @time WHERE agentid = @agentid; ", new
            {
                value = item.Value,
                time = item.Time,
                agentid = item.AgentId
            });
        }

        public IList<HddMetricDatabase> GetAll()
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            return connection.Query<HddMetricDatabase>("SELECT * FROM hddmetrics").ToList();

        }

        public IList<HddMetricDatabase> GetById(long id)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            return connection.Query<HddMetricDatabase>("SELECT * FROM hddmetrics WHERE agentid=@agentid", new
            {
                agentid = id
            }).ToList();
        }

        /// <summary>
        /// Получение данных по нагрузке на ЦП за период
        /// </summary>
        /// <param name="timeFrom">Время начала периода</param>
        /// <param name="timeTo">Время окончания периода</param>
        /// <returns></returns>
        public IList<HddMetricDatabase> GetByTimePeriod(long id, TimeSpan timeFrom, TimeSpan timeTo)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            return connection.Query<HddMetricDatabase>("SELECT * FROM hddmetrics where agentid=@agentid and time >= @timeFrom and time <= @timeTo", new
            {
                agentid = id,
                timeFrom = timeFrom.TotalSeconds,
                timeTo = timeTo.TotalSeconds
            }).ToList();

        }

        public TimeSpan GetLastMetricTime(long id)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            dynamic queryResult = connection.QueryFirstOrDefault<dynamic>("SELECT MAX(time) as time FROM hddmetrics", new
            {
                agentid = id,
            });

            return queryResult.time == null ? new TimeSpan() : TimeSpan.FromSeconds(Convert.ToInt64(queryResult.time));
        }
    }

}

