using System;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using HumanResourceManagement.Models;

namespace HumanResourceManagement.Services
{
    public class WeatherCacheService
    {
        private const string WeatherCacheKey = "weather_cache";

        private readonly IWeatherService weatherService;
        private readonly IPreferences preferences;

        public WeatherCacheService(IWeatherService weatherService, IPreferences preferences)
        {
            this.weatherService = weatherService;
            this.preferences = preferences;
        }

        public async Task<WeatherData> GetLatestWeatherAsync(double lat, double lon)
        {
            CachedWeather? saved = ReadSaved();

            try
            {
                WeatherData fresh = await weatherService.GetWeatherAsync(lat, lon);
                Save(fresh);
                return fresh;
            }
            catch (WeatherServiceException) when (saved != null)
            {
                return saved.Weather;
            }
        }

        private CachedWeather? ReadSaved()
        {
            string json = preferences.Get(WeatherCacheKey, "");
            if (string.IsNullOrEmpty(json)) return null;
            try
            {
                return JsonSerializer.Deserialize<CachedWeather>(json);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private void Save(WeatherData weather)
        {
            var cached = new CachedWeather
            {
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                Weather = weather
            };
            preferences.Set(WeatherCacheKey, JsonSerializer.Serialize(cached));
        }
    }
}