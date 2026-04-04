using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WeatherApp_Demo.Data;
using WeatherApp_Demo.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
namespace WeatherApp_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        public MainWindow()
        {
            InitializeComponent();
            LoadDataFromDatabase();
            SetupTimer(); // Khởi động bộ đếm
            this.DataContext = this; // Binding data
            LoadHourlyCards();
            LoadRealWeatherData("Hanoi");
        }
        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(5); // Đặt mốc 5 phút
            _timer.Tick += (s, e) =>
            {
                LoadDataFromDatabase(); // Gọi hàm lấy dữ liệu mới từ DB
                                        // Có thể thêm hiệu ứng nhỏ để người dùng biết data đã update
                this.Title = "Cập nhật lúc: " + DateTime.Now.ToString("HH:mm:ss");
            };
            _timer.Start();
        }

        private void LoadDataFromDatabase()
        {
            using (var db = new WeatherDbContext())
            {
                // 1. Lấy dữ liệu hiện tại
                CurrentWeather = db.CurrentWeathers.FirstOrDefault();

                // Gán DataContext để XAML có thể hiểu các biến {Binding ...}
                this.DataContext = null; // Reset để UI cập nhật
                this.DataContext = this;

                // 2. Lấy 30 bản ghi lịch sử (như Ngày 4)
                var data = db.DailyForecasts.OrderByDescending(x => x.ID).Take(30).ToList();
                ForecastList.ItemsSource = data;
            }
        }
        private void LoadHourlyCards()
        {
            HourlyCards.Clear();
            // Nạp dữ liệu mô phỏng. Lưu ý thẻ đầu tiên có IsActive = true
            HourlyCards.Add(new HourlyForecastItem { Time = "Bây giờ", Temp = 27, IconSource = "https://cdn-icons-png.flaticon.com/512/1163/1163624.png", IsActive = true });
            HourlyCards.Add(new HourlyForecastItem { Time = "15:00", Temp = 26, IconSource = "https://cdn-icons-png.flaticon.com/512/414/414927.png" });
            HourlyCards.Add(new HourlyForecastItem { Time = "16:00", Temp = 26, IconSource = "https://cdn-icons-png.flaticon.com/512/414/414927.png" });
            HourlyCards.Add(new HourlyForecastItem { Time = "17:00", Temp = 25, IconSource = "https://cdn-icons-png.flaticon.com/512/414/414927.png" });
            HourlyCards.Add(new HourlyForecastItem { Time = "18:00", Temp = 25, IconSource = "https://cdn-icons-png.flaticon.com/512/414/414927.png" });
            HourlyCards.Add(new HourlyForecastItem { Time = "19:00", Temp = 24, IconSource = "https://cdn-icons-png.flaticon.com/512/414/414927.png" });
        }
        private void HourlyScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (e.Delta > 0)
            {
                scrollViewer.LineLeft(); // Cuộn sang trái khi lăn lên
            }
            else
            {
                scrollViewer.LineRight(); // Cuộn sang phải khi lăn xuống
            }
            e.Handled = true; // Báo hiệu đã xử lý xong, không cuộn dọc cả trang web nữa
        }
        private async void LoadRealWeatherData(string cityName)
        {
            var service = new WeatherService();
            try
            {
                var data = await service.GetWeatherAsync(cityName);

                // Cập nhật lên giao diện
                txtCityName.Text = data.name;
                txtTemp.Text = $"{Math.Round(data.main.temp)}°";
                txtDescription.Text = data.weather[0].description;

                // Cập nhật các Gauge (Độ ẩm, gió...)
                this.CurrentWeather = new Weather
                {
                    Temperature = data.main.temp,
                    Humidity = data.main.humidity,
                    
                    // ... gán tiếp các thông số khác
                };

                this.DataContext = null;
                this.DataContext = this;
            }
            catch
            {
                MessageBox.Show("Không tìm thấy thành phố này! Vui lòng thử lại.");
            }
        }
        // 1. Khi nhấn nút Search
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSearch();
        }

        // 2. Khi nhấn phím Enter trong ô nhập
        private void txtSearchCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ExecuteSearch();
            }
        }

        // 3. Hàm thực hiện tìm kiếm chung
        private void ExecuteSearch()
        {
            string cityName = txtSearchCity.Text.Trim();
            if (!string.IsNullOrEmpty(cityName))
            {
                // Gọi lại hàm Load dữ liệu với tên thành phố mới
                LoadRealWeatherData(cityName);
            }
        }
        // Thêm namespace này lên đầu file: using WeatherApp_Demo.Models;
        public Weather? CurrentWeather { get; set; }
        public ChartValues<double> HourlyTempValues { get; set; } = new ChartValues<double>();
        public List<string> HourlyLabels { get; set; } = new List<string>();

        public ObservableCollection<HourlyForecastItem> HourlyCards { get; set; } = new ObservableCollection<HourlyForecastItem>();
       

    }


}