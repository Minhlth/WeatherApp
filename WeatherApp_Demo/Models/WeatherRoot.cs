namespace WeatherApp_Demo.Models
{
    // Lớp chính chứa toàn bộ dữ liệu trả về từ API
    public class WeatherRoot
    {
        public MainData main { get; set; }
        public WindData wind { get; set; }
        public WeatherDesc[] weather { get; set; }
        public string name { get; set; }
    }

    // Chứa nhiệt độ và độ ẩm
    public class MainData
    {
        public double temp { get; set; }
        public int humidity { get; set; }
    }

    // Chứa tốc độ gió
    public class WindData
    {
        public double speed { get; set; }
    }

    // Chứa mô tả thời tiết (Trời quang, mây rải rác...)
    public class WeatherDesc
    {
        public string description { get; set; }
    }
}