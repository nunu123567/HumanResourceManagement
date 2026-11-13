using System.Threading.Tasks;
using HumanResourceManagement.Models;

namespace HumanResourceManagement.Services
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeatherAsync(double latitude, double longitude);
    }
}