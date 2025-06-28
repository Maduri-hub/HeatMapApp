using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using HeatMapApp;
using HeatMapApp.Services;
using System.IO;

namespace HeatMapApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<LocationService>();
            builder.Services.AddSingleton<LocationDatabase>(_ =>
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db3");
                return new LocationDatabase(dbPath);
            });

            builder.Services.AddSingleton<App>();

            return builder.Build();
        }
    }
}
