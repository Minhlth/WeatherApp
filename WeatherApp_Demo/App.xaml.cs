using System.Linq;
using System.Windows;
using WeatherApp_Demo.Data;
using WeatherApp_Demo.Models;

namespace WeatherApp_Demo
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var db = new WeatherDbContext())
            {
                // Tự động tạo file weather.db nếu chưa tồn tại
                db.Database.EnsureCreated();

                // Kiểm tra xem đã có dữ liệu chưa, nếu chưa thì mới thêm vào
                if (!db.CurrentWeathers.Any())
                {
                    // 1. Thêm dữ liệu thời tiết hiện tại (Giống ảnh bạn gửi)
                    db.CurrentWeathers.Add(new Weather
                    {
                        LocationName = "Cao Quán",
                        Temperature = 27,
                        Condition = "Nhiều mây",
                        Humidity = 65,
                        UVIndex = 1,
                        AQI = 44,

                    });

                    // 2. Thêm dữ liệu dự báo mẫu cho 5 ngày
                    db.DailyForecasts.AddRange(
                        new DailyForecast { DayName = "Hôm nay", TempMin = 21, TempMax = 27, IconPath = "rain.png" },
                        new DailyForecast { DayName = "Ngày mai", TempMin = 22, TempMax = 28, IconPath = "cloudy.png" },
                        new DailyForecast { DayName = "Thứ 2", TempMin = 22, TempMax = 30, IconPath = "sunny.png" }
                    );

                    db.SaveChanges(); // Lưu tất cả vào file .db
                                      // Trong hàm InitializeDatabase của App.xaml.cs
                    if (db.DailyForecasts.Count() < 30)
                    {
                        for (int i = 1; i <= 30; i++)
                        {
                            db.DailyForecasts.Add(new DailyForecast
                            {
                                DayName = $"Lần đo {i}",
                                TempMin = 20 + (i % 5),
                                TempMax = 25 + (i % 7),
                                IconPath = "stats.png"
                            });
                        }
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}