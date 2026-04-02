using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp_Demo.Models;
public class WeatherService
{
    private const string ApiKey = "31c087ecee8d9e0fd850e2e23a94a94e";
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

    public async Task<WeatherRoot> GetWeatherAsync(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            // units=metric để lấy độ C, lang=vi để lấy mô tả tiếng Việt
            string url = $"{BaseUrl}?q={city}&appid={ApiKey}&units=metric&lang=vi";
            var response = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<WeatherRoot>(response);
        }
    }
}