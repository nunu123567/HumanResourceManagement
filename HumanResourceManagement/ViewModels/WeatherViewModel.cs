using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HumanResourceManagement.Models;
using HumanResourceManagement.Services;

namespace HumanResourceManagement.ViewModels
{
    public partial class WeatherViewModel : ObservableObject
    {
        private readonly WeatherCacheService weatherCacheService;

        [ObservableProperty]
        private partial string Heading { get; set; } = "Weather Forecast (Vaasa)";

        [ObservableProperty]
        private partial string TemperatureText { get; set; } = "-- °C";

        [ObservableProperty]
        private partial string WindText { get; set; } = "-- m/s";

        [ObservableProperty]
        private partial string KitesurfAlert { get; set; } = "";

        [ObservableProperty]
        private partial bool ShowKitesurfAlert { get; set; }

        [ObservableProperty]
        private partial bool IsBusy { get; set; }

        [ObservableProperty]
        private partial string Status { get; set; } = "";

        public WeatherViewModel(WeatherCacheService weatherCacheService)
        {
            this.weatherCacheService = weatherCacheService;
        }

        [RelayCommand]
        public async Task LoadWeatherAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            Status = "";

            try
            {
                // Vaasa coordinates: Latitude 63.10, Longitude 21.62
                WeatherData weather = await weatherCacheService.GetLatestWeatherAsync(63.10, 21.62);

                if (weather?.Current != null)
                {
                    TemperatureText = $"{weather.Current.Temperature2m}°C";
                    WindText = $"Wind speed: {weather.Current.WindSpeed10m} m/s";

                    // Kitesurf alert: if wind is over 5 m/s
                    if (weather.Current.WindSpeed10m > 5.0)
                    {
                        KitesurfAlert = "🪁 Kitesurf weather!";
                        ShowKitesurfAlert = true;
                    }
                    else
                    {
                        KitesurfAlert = "";
                        ShowKitesurfAlert = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}