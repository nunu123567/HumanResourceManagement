using HumanResourceManagement.Services;
using HumanResourceManagement.ViewModels;
using HumanResourceManagement.Views;
using Microsoft.Extensions.Logging;

namespace HumanResourceManagement
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<TodoStore>();          
            builder.Services.AddTransient<AddTaskViewModel>();
            builder.Services.AddTransient<AddTaskPage>();
            builder.Services.AddTransient<TodoListViewModel>();
            builder.Services.AddTransient<TodoListPage>();
            builder.Services.AddTransient<WeatherViewModel>();
            builder.Services.AddTransient<WeatherPage>();
            builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
            {
                client.BaseAddress = new Uri("https://api.open-meteo.com/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });
            builder.Services.AddTransient<WeatherCacheService>();
            builder.Services.AddSingleton<IPreferences>(Preferences.Default);
            return builder.Build();
        }
    }
}
