using System.Text.Json.Serialization;

namespace HumanResourceManagement.Models
{
    public class WeatherData
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("current")]
        public CurrentWeather? Current { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("time")]
        public string Time { get; set; } = "";

        [JsonPropertyName("temperature_2m")]
        public double Temperature2m { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed10m { get; set; }
    }
}