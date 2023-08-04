using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestApp.Weather;

public class WeatherService
{
    private const string ApiKey = "2aa036605a200a74b2d80ab621544076";
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

    public async Task<WeatherData> GetWeatherData(string city)
    {
        string url = $"{BaseUrl}?q={city}&appid={ApiKey}&units=metric";

        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonResult = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResult);
                return weatherData;
            }
            catch (HttpRequestException ex)
            {
                // Handle any exceptions or errors.
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
