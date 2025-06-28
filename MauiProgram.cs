using HeatMapApp.Services;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
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

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db3");
            builder.Services.AddSingleton(new LocationDatabase(dbPath));
            builder.Services.AddSingleton<LocationService>();
            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}
