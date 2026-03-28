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
        // Thêm namespace này lên đầu file: using WeatherApp_Demo.Models;
        public Weather? CurrentWeather { get; set; }

    }


}