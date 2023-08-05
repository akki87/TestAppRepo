using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TestApp.Weather;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isWindowActive = false;
        private DispatcherTimer timer;  
        private object dataLock = new object();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }
        
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // You can add default city or leave it empty.
            // In this example, we are not setting a default city.
             //await GetData();
            Main();
        }
        private async  Task GetData()
        {
            string city = "Hyderabad";
            WeatherService weatherService = new WeatherService();
            WeatherData weatherData = await weatherService.GetWeatherData(city);
            // Update the UI with weather data
            UpdateUI(weatherData);
        }
        private void UpdateUI(WeatherData weatherData)
        {
            // Update UI elements with weather details
            mainTextBlock.Text = $"Today  {weatherData.main.temp} °C ";
            nameTextBlock.Text = $"in {weatherData.name}";
            weatherTextBlock.Text = $"{weatherData.weather[0].main}";
            humidityTextBlock.Text = $"{weatherData.main.humidity}% humidity";
            feelsLikeTextBlock.Text = $"Feels like {(weatherData.main.temp + 2).ToString().Substring(0,4)} °C";
            HighTextBlock.Text = $"Max temp {(weatherData.main.temp + 2).ToString().Substring(0,4)}°C";
            LowTextBlock.Text = $"Min temp {(weatherData.main.temp - 12).ToString().Substring(0,4)}°C";

            string WindDirection(int dir)
            {
                string[] directions = { "North", "North-East", "East", "South-East", "South", "South-West", "West", "North-West" };
                int index = (int)Math.Round((Convert.ToDecimal(dir) % 360) / 45) % 8;
                return directions[index];
            }
            windTextBlock.Text = $"{weatherData.wind.speed}m/s , towards {WindDirection(weatherData.wind.deg)}";
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Calculate the font size based on window dimensions
            double windowWidth = this.ActualWidth;
            double windowHeight = this.ActualHeight;
            double fontSize = (windowWidth + windowHeight) / 50; // Adjust this value to your preference

            // Limit the font size to a minimum value to prevent it from becoming too small
            if (fontSize < 12)
            {
                fontSize = 12;
            }

            // Set the calculated font size to the TextBlock
            mainTextBlock.FontSize = fontSize;
            nameTextBlock.FontSize = fontSize;
            weatherTextBlock.FontSize = fontSize;
            humidityTextBlock.FontSize = fontSize;
            feelsLikeTextBlock.FontSize = fontSize < 12 ? fontSize : fontSize-8 ;
            HighTextBlock.FontSize = fontSize < 12 ? fontSize : fontSize - 8;
            LowTextBlock.FontSize = fontSize < 12 ? fontSize : fontSize - 8;
            windTextBlock.FontSize = fontSize - 10;

        }
        async Task Main()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += Timer_Tick;
            timer.Start();
            // Start the first data fetch and update UI
            await GetData();
            

        }
        private void Window_Activated(object sender, EventArgs e)
        {
            // Window becomes active
            isWindowActive = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            // Window becomes inactive
            isWindowActive = false;
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            // Update the data every 10 seconds using Dispatcher.Invoke
            // Dispatcher.Invoke ensures UI updates are done on the UI thread
            await Dispatcher.Invoke(async () =>
            {
                lock (dataLock)
                {
                    GetData();
                }
            });
        }



    }

}
