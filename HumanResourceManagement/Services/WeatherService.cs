using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using HumanResourceManagement.Models;

namespace HumanResourceManagement.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient http;

        public WeatherService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<WeatherData> GetWeatherAsync(double latitude, double longitude)
        {
            string lat = latitude.ToString(CultureInfo.InvariantCulture);
            string lon = longitude.ToString(CultureInfo.InvariantCulture);

            // wind speed is requested in m/s instead of km/h
            string url = $"v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,wind_speed_10m&wind_speed_unit=ms";

            try
            {
                using HttpResponseMessage response = await http.GetAsync(url);
                Debug.WriteLine($"GET {url} -> {(int)response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new WeatherServiceException($"The weather server answered {(int)response.StatusCode}.");
                }

                var weather = await response.Content.ReadFromJsonAsync<WeatherData>();
                if (weather == null || weather.Current == null)
                {
                    throw new WeatherServiceException("The response did not contain weather data.");
                }

                return weather;
            }
            catch (HttpRequestException ex)
            {
                throw new WeatherServiceException("No connection to the weather server. Check the network.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new WeatherServiceException("The weather server did not answer in time.", ex);
            }
            catch (JsonException ex)
            {
                throw new WeatherServiceException("The weather server sent invalid data.", ex);
            }
        }
    }
}