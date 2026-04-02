using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WeatherApp_Demo.Helpers
{
    public class WeatherIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string condition = value?.ToString() ?? "Sunny";
            // Trả về đường dẫn ảnh tương ứng với trạng thái từ Database
            return condition switch
            {
                "Nhiều mây" => "/Assets/cloudy.png",
                "Mưa" => "/Assets/heavy-rain.png",
                "Nắng" => "/Assets/sun.png",
                _ => "/Assets/sun.png"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
