using MetricsAgent.Models;
using System.Data.SQLite;

namespace MetricsAgent.Services.Implemetation
{
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

        public void Create(DotNetMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            // Создаём команду
            using var cmd = new SQLiteCommand(connection);
            // Прописываем в команду SQL-запрос на вставку данных
            cmd.CommandText = "INSERT INTO dotnetmetrics(value, time) VALUES(@value, @time)";
            // Добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@value", item.Value);
            // В таблице будем хранить время в секундах
            cmd.Parameters.AddWithValue("@time", item.Time);
            // подготовка команды к выполнению
            cmd.Prepare();
            // Выполнение команды
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            // Прописываем в команду SQL-запрос на удаление данных
            cmd.CommandText = "DELETE FROM dotnetmetrics WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<DotNetMetric> GetAll()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            // Прописываем в команду SQL-запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM dotnetmetrics";
            var returnList = new List<DotNetMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // Пока есть что читать — читаем
                while (reader.Read())
                {
                    // Добавляем объект в список возврата
                    returnList.Add(new DotNetMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = reader.GetInt32(2)
                    });
                }
            }
            return returnList;
        }

        public DotNetMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM dotnetmetrics WHERE id=@id";
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // Если удалось что-то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new DotNetMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = reader.GetInt32(2)
                    };
                }
                else
                {
                    // Не нашлась запись по идентификатору, не делаем ничего
                    return null;
                }
            }
        }

        /// <summary>
        /// Получение данных по ошибкам DotNet за период
        /// </summary>
        /// <param name="timeFrom">Время начала периода</param>
        /// <param name="timeTo">Время окончания периода</param>
        /// <returns></returns>
        public IList<DotNetMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            // Прописываем в команду SQL-запрос на получение всех данных за период из таблицы
            cmd.CommandText = "SELECT * FROM dotnetmetrics where time >= @timeFrom and time <= @timeTo";
            cmd.Parameters.AddWithValue("@timeFrom", timeFrom.TotalSeconds);
            cmd.Parameters.AddWithValue("@timeTo", timeTo.TotalSeconds);
            var returnList = new List<DotNetMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // Пока есть что читать — читаем
                while (reader.Read())
                {
                    // Добавляем объект в список возврата
                    returnList.Add(new DotNetMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = reader.GetInt32(2)
                    });
                }
            }
            return returnList;
        }

        public void Update(DotNetMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            using var cmd = new SQLiteCommand(connection);
            // Прописываем в команду SQL-запрос на обновление данных
            cmd.CommandText = "UPDATE dotnetmetrics SET value = @value, time = @time WHERE id = @id; ";
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
