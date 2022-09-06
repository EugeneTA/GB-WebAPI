using Dapper;
using MetricsAgent.Models;
using MetricsAgent.Options;
using Microsoft.Extensions.Options;
using System.Data.SQLite;

namespace MetricsAgent.Services.Implemetation
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        //private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

        #region Services

        private readonly IOptions<DatabaseOptions> _databaseOptions;

        #endregion

        public HddMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(HddMetric item)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            connection.Execute("INSERT INTO hddmetrics(value, time) VALUES(@value, @time)", new
            {
                value = item.Value,
                time = item.Time
            });


            //// Using raw connection
            //using var connection = new SQLiteConnection(ConnectionString);
            //connection.Open();
            //// Создаём команду
            //using var cmd = new SQLiteCommand(connection);
            //// Прописываем в команду SQL-запрос на вставку данных
            //cmd.CommandText = "INSERT INTO hddmetrics(value, time) VALUES(@value, @time)";
            //// Добавляем параметры в запрос из нашего объекта
            //cmd.Parameters.AddWithValue("@value", item.Value);
            //// В таблице будем хранить время в секундах
            //cmd.Parameters.AddWithValue("@time", item.Time);
            //// подготовка команды к выполнению
            //cmd.Prepare();
            //// Выполнение команды
            //cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            connection.Execute("DELETE FROM hddmetrics WHERE id=@id", new
            {
                id = id
            });

            ////using var connection = new SQLiteConnection(ConnectionString);
            //connection.Open();
            //using var cmd = new SQLiteCommand(connection);
            //// Прописываем в команду SQL-запрос на удаление данных
            //cmd.CommandText = "DELETE FROM hddmetrics WHERE id=@id";
            //cmd.Parameters.AddWithValue("@id", id);
            //cmd.Prepare();
            //cmd.ExecuteNonQuery();
        }

        public void Update(HddMetric item)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            connection.Execute("UPDATE hddmetrics SET value = @value, time = @time WHERE id = @id; ", new
            {
                value = item.Value,
                time = item.Time,
                id = item.Id
            });

            ////using var connection = new SQLiteConnection(ConnectionString);
            //using var cmd = new SQLiteCommand(connection);
            //// Прописываем в команду SQL-запрос на обновление данных
            //cmd.CommandText = "UPDATE hddmetrics SET value = @value, time = @time WHERE id = @id; ";
            //cmd.Parameters.AddWithValue("@id", item.Id);
            //cmd.Parameters.AddWithValue("@value", item.Value);
            //cmd.Parameters.AddWithValue("@time", item.Time);
            //cmd.Prepare();
            //cmd.ExecuteNonQuery();
        }

        public IList<HddMetric> GetAll()
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            return connection.Query<HddMetric>("SELECT * FROM hddmetrics").ToList();

            ////using var connection = new SQLiteConnection(ConnectionString);
            //connection.Open();
            //using var cmd = new SQLiteCommand(connection);
            //// Прописываем в команду SQL-запрос на получение всех данных из таблицы
            //cmd.CommandText = "SELECT * FROM hddmetrics";
            //var returnList = new List<HddMetric>();
            //using (SQLiteDataReader reader = cmd.ExecuteReader())
            //{
            //    // Пока есть что читать — читаем
            //    while (reader.Read())
            //    {
            //        // Добавляем объект в список возврата
            //        returnList.Add(new HddMetric
            //        {
            //            Id = reader.GetInt32(0),
            //            Value = reader.GetInt32(1),
            //            Time = reader.GetInt32(2)
            //        });
            //    }
            //}
            //return returnList;
        }

        public HddMetric GetById(int id)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            return connection.QuerySingle<HddMetric>("SELECT * FROM hddmetrics WHERE id=@id", new
            {
                id = id
            });

            ////using var connection = new SQLiteConnection(ConnectionString);
            //connection.Open();
            //using var cmd = new SQLiteCommand(connection);
            //cmd.CommandText = "SELECT * FROM hddmetrics WHERE id=@id";
            //using (SQLiteDataReader reader = cmd.ExecuteReader())
            //{
            //    // Если удалось что-то прочитать
            //    if (reader.Read())
            //    {
            //        // возвращаем прочитанное
            //        return new HddMetric
            //        {
            //            Id = reader.GetInt32(0),
            //            Value = reader.GetInt32(1),
            //            Time = reader.GetInt32(2)
            //        };
            //    }
            //    else
            //    {
            //        // Не нашлась запись по идентификатору, не делаем ничего
            //        return null;
            //    }
            //}
        }

        /// <summary>
        /// Получение данных по нагрузке на HDD за период
        /// </summary>
        /// <param name="timeFrom">Время начала периода</param>
        /// <param name="timeTo">Время окончания периода</param>
        /// <returns></returns>
        public IList<HddMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            // Using connection string from app settings file (appsettings.json)
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

            // Using Dapper nuget packet
            return connection.Query<HddMetric>("SELECT * FROM hddmetrics where time >= @timeFrom and time <= @timeTo", new
            {
                timeFrom = timeFrom.TotalSeconds,
                timeTo = timeTo.TotalSeconds
            }).ToList();

            ////using var connection = new SQLiteConnection(ConnectionString);
            //connection.Open();
            //using var cmd = new SQLiteCommand(connection);
            //// Прописываем в команду SQL-запрос на получение всех данных за период из таблицы
            //cmd.CommandText = "SELECT * FROM hddmetrics where time >= @timeFrom and time <= @timeTo";
            //cmd.Parameters.AddWithValue("@timeFrom", timeFrom.TotalSeconds);
            //cmd.Parameters.AddWithValue("@timeTo", timeTo.TotalSeconds);
            //var returnList = new List<HddMetric>();
            //using (SQLiteDataReader reader = cmd.ExecuteReader())
            //{
            //    // Пока есть что читать — читаем
            //    while (reader.Read())
            //    {
            //        // Добавляем объект в список возврата
            //        returnList.Add(new HddMetric
            //        {
            //            Id = reader.GetInt32(0),
            //            Value = reader.GetInt32(1),
            //            Time = reader.GetInt32(2)
            //        });
            //    }
            //}
            //return returnList;
        }


    }
}
