namespace HumanResourceManagement.Models
{
    public class CachedWeather
    {
        public string Timestamp { get; set; } = "";
        public WeatherData Weather { get; set; } = new();
    }
}