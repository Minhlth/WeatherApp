using Microsoft.EntityFrameworkCore;
using WeatherApp_Demo.Models;
namespace WeatherApp_Demo.Data
{
    public class WeatherDbContext : DbContext
    {
        public DbSet<Weather> CurrentWeathers { get; set; }
        public DbSet<DailyForecast> DailyForecasts { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=weather.db");
        }

    }
   
}
