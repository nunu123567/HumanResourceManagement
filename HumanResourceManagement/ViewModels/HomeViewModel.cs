using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HumanResourceManagement.Models;
using HumanResourceManagement.Services;

namespace HumanResourceManagement.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly WeatherCacheService weatherCacheService;
        private readonly TodoStore todoRepository; // Or your TodoStore reference

        [ObservableProperty]
        public partial ObservableCollection<TodoItem> TopTodos { get; set; } = new();

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

        public HomeViewModel(WeatherCacheService weatherCacheService, TodoStore todoRepository)
        {
            this.weatherCacheService = weatherCacheService;
            this.todoRepository = todoRepository;
        }

        [RelayCommand]
        public async Task LoadHomeDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            Status = "";

            try
            {
                // 1. Load Top 3 Unfinished To-Dos (Highest Priority first, then Earliest Due Date)
                var top3 = todoRepository.Items
                    .Where(t => !t.IsDone)
                    .OrderByDescending(t => t.Priority)
                    .ThenBy(t => t.DueDate)
                    .Take(3)
                    .ToList();
                

                TopTodos.Clear();
                foreach (var todo in top3)
                {
                    TopTodos.Add(todo);
                }

                // 2. Load Weather for Vaasa (Latitude 63.10, Longitude 21.62) [cite: ]
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
                Status = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}