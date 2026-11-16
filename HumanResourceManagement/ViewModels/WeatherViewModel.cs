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
        private readonly IWeatherService weatherService;

        [ObservableProperty]
        public partial string Heading { get; set; } = "Weather Forecast (Vaasa)";

        [ObservableProperty]
        public partial string TemperatureText { get; set; } = "-- °C";

        [ObservableProperty]
        public partial string WindText { get; set; } = "-- m/s";

        [ObservableProperty]
        public partial string KitesurfAlert { get; set; } = "";

        [ObservableProperty]
        public partial bool ShowKitesurfAlert { get; set; }

        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        [ObservableProperty]
        public partial string Status { get; set; } = "";

        public WeatherViewModel(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
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
                // Note: If your IWeatherService method name differs (e.g. GetWeatherAsync), update it here.
                WeatherData weather = await weatherService.GetWeatherAsync(63.10, 21.62);

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