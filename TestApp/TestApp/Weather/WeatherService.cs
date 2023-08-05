using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using TestApp.Weather;

public class WeatherService
{

    public async Task<WeatherData> GetWeatherData(string city)
    {
        string url = $"{ConfigurationManager.AppSettings.Get("apiurl")}?q={city}&appid={ConfigurationManager.AppSettings.Get("apikey")}&units=metric";

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
