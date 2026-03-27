using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp_Demo.Models
{
    public class Weather
    {
        public int ID { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Condination { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public int UVIndex { get; set; }
        public int AQI { get; set; }

     }
    public class DailyForecast
    {
        public string DayName { get; set; } = string.Empty; 
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public string IconPath { get; set; } = string.Empty;
    }
}
