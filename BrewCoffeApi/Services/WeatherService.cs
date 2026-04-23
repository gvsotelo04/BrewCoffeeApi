using BrewCoffeeApi.Model;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;

namespace BrewCoffeeApi.Services
{


    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherSettings _settings;
        
        public WeatherService(HttpClient http, IOptions<OpenWeatherSettings> options)
        {
            _httpClient = http; // new HttpClient();
            _settings = options.Value;
        }

        public async Task<double> GetTemperatureAsync(string city)
        {            
            try
            {

                var url = $"{_settings.BaseUrl}?q={city}&appid={_settings.ApiKey}&units=metric";

                var response = await _httpClient.GetStringAsync(url);

                using var doc = JsonDocument.Parse(response);

                var temp = doc.RootElement
                              .GetProperty("main")
                              .GetProperty("temp")
                              .GetDouble();

                return temp;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        
    }
}
