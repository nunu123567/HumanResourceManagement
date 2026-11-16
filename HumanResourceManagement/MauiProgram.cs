using HumanResourceManagement.Data;
using HumanResourceManagement.Services;
using HumanResourceManagement.ViewModels;
using HumanResourceManagement.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;

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

            // --- Database & SQLite (Part 1 & 3) ---
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "todos.db");
            Debug.WriteLine($"Database: {dbPath}");
            builder.Services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}")
                       .LogTo(message => Debug.WriteLine(message), LogLevel.Information));

            // --- Repositories & Stores ---
            builder.Services.AddSingleton<ITodoRepository, EfTodoRepository>();
            builder.Services.AddSingleton<TodoStore>();

            // --- ViewModels & Pages ---
            builder.Services.AddTransient<AddTaskViewModel>();
            builder.Services.AddTransient<AddTaskPage>();
            builder.Services.AddTransient<TodoListViewModel>();
            builder.Services.AddTransient<TodoListPage>();
            builder.Services.AddTransient<WeatherViewModel>();
            builder.Services.AddTransient<WeatherPage>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();

            // --- Weather API & Caching ---
            builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
            {
                client.BaseAddress = new Uri("https://api.open-meteo.com/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });
            builder.Services.AddSingleton<IPreferences>(Preferences.Default);

            // --- Build App ---
            var app = builder.Build();

            // Create database file & tables if they don't exist
            using (AppDbContext db = app.Services
                       .GetRequiredService<IDbContextFactory<AppDbContext>>()
                       .CreateDbContext())
            {
                db.Database.EnsureCreated();
            }

            return app;
        }
    }
}