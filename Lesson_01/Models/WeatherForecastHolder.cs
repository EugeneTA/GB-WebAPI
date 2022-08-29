using Lesson_01.Classes;

namespace Lesson_01.Models
{
    public class WeatherForecastHolder
    {
        private List<WeatherForecast> _weatherForecastValues;

        #region Constructor
        public WeatherForecastHolder()
        {
            // Initialize weather forecast list
            _weatherForecastValues = new List<WeatherForecast>();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Добавить новый показатель температуры
        /// </summary>
        /// <param name="date">Дата фиксации показателя температуры</param>
        /// <param name="temperatureC">Показатель температуры</param>
        public void Add(DateTime date, int temperatureC)
        {
            _weatherForecastValues.Add(new WeatherForecast() { Date = date, TemperatureC = temperatureC });
        }

        /// <summary>
        /// Обновить показатель температуры
        /// </summary>
        /// <param name="date">Дата фиксации показания температуры</param>
        /// <param name="temperatureC">Новый показатель температуры</param>
        /// <returns>Результат выполнения операции</returns>
        public bool Update(DateTime date, int temperatureC)
        {
            foreach (var item in _weatherForecastValues)
            {
                if (item.Date == date)
                {
                    item.TemperatureC = temperatureC;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Получить показатели температуры за временной период
        /// </summary>
        /// <param name="dateFrom">Начальная дата</param>
        /// <param name="dateTo">Конечная дата</param>
        /// <returns>Коллекция показателей температуры</returns>
        public List<WeatherForecast> Get(DateTime dateFrom, DateTime dateTo)
        {
            return _weatherForecastValues.FindAll(item => item.Date >= dateFrom && item.Date <= dateTo);
        }

        /// <summary>
        /// Удалить показатель температуты на дату
        /// </summary>
        /// <param name="date">Дата фиксации показателя температуры</param>
        /// <returns>Результат выполнения операции</returns>
        public bool Delete(DateTime date)
        {
            foreach (var item in _weatherForecastValues)
            {
                if (item.Date == date)
                {
                    _weatherForecastValues.Remove(item);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Удалить показатель температуты за промежуток времени
        /// </summary>
        /// <param name="dateFrom">Начальная дата</param>
        /// <param name="dateTo">Конечная дата</param>
        /// <returns>Результат выполнения операции</returns>
        public bool Delete(DateTime fromDate, DateTime toDate)
        {

            var result = this.Get(fromDate, toDate);

            if (result != null && result.Count > 0)
            {
                foreach(var item in result)
                {
                    _weatherForecastValues.Remove(item);
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
