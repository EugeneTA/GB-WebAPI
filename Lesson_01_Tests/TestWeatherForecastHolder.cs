using Lesson_01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_01_Tests
{
    internal class TestWeatherForecastHolder
    {
        private static readonly Lazy<WeatherForecastHolder> _instance = new Lazy<WeatherForecastHolder>(() => new WeatherForecastHolder());

        public static WeatherForecastHolder Instance
        {
            get { return _instance.Value; }
        }
    }
}
