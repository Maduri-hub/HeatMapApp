using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Devices.Sensors;
using HeatMapApp.Services;
using HeatMapApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

#if IOS
using MapKit;
using CoreGraphics;
using UIKit;
#endif

namespace HeatMapApp
{
    public partial class MainPage : ContentPage
    {
        private readonly LocationService _locationService;
        private readonly LocationDatabase _database;

        public MainPage(LocationService locationService, LocationDatabase database)
        {
            InitializeComponent();
            _locationService = locationService;
            _database = database;

#if IOS
            Loaded += async (_, _) =>
            {
                if (MyMap?.Handler?.PlatformView is MKMapView nativeMap)
                {
                    nativeMap.ShowsUserLocation = true;
                }

                await CenterMapOnSuwaneeAsync();
                await RenderHeatMapAsync();
                await DrawMovementPolylineAsync();
            };
#else
            _ = CenterMapOnSuwaneeAsync();
            _ = RenderHeatMapAsync();
            _ = DrawMovementPolylineAsync();
#endif
        }

        private async Task CenterMapOnSuwaneeAsync()
        {
            var center = new Location(33.9604, -84.1065); // Center between Suwanee and Decatur
            var span = MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(25));
            MyMap.MoveToRegion(span);
        }

        private async Task RenderHeatMapAsync()
        {
            var locations = await _database.GetLocationsAsync();

            var grouped = locations
                .GroupBy(loc => new { Lat = Math.Round(loc.Latitude, 3), Lng = Math.Round(loc.Longitude, 3) });

            MyMap.MapElements.Clear();

            foreach (var group in grouped)
            {
                double intensity = group.Count();
                float alpha = (float)Math.Min(0.2 + intensity * 0.1, 0.8);

                var circle = new Circle
                {
                    Center = new Location(group.Key.Lat, group.Key.Lng),
                    Radius = new Distance(150),
                    StrokeColor = Colors.Transparent,
                    FillColor = Colors.Red.WithAlpha(alpha)
                };

                MyMap.MapElements.Add(circle);
            }
        }

        private async Task DrawMovementPolylineAsync()
        {
            var locations = await _database.GetLocationsAsync();

            if (locations.Count < 2)
                return;

            var sorted = locations.OrderBy(l => l.Timestamp).ToList();

            var polyline = new Polyline
            {
                StrokeColor = Colors.Blue,
                StrokeWidth = 3
            };

            foreach (var point in sorted)
            {
                polyline.Geopath.Add(new Location(point.Latitude, point.Longitude));
            }

            MyMap.MapElements.Add(polyline);
        }

        private async void OnAddTestLocationClicked(object sender, EventArgs e)
        {
            var testPoints = new List<LocationModel>
            {
                new LocationModel { Latitude = 34.0515, Longitude = -84.0713, Timestamp = DateTime.UtcNow.AddMinutes(-10) }, // Suwanee
                new LocationModel { Latitude = 33.9860, Longitude = -84.0916, Timestamp = DateTime.UtcNow.AddMinutes(-5) }, // Duluth
                new LocationModel { Latitude = 33.7748, Longitude = -84.2963, Timestamp = DateTime.UtcNow } // Decatur
            };

            foreach (var point in testPoints)
            {
                await _database.SaveLocationAsync(point);
            }

            await RenderHeatMapAsync();
            await DrawMovementPolylineAsync();
        }
        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            await RenderHeatMapAsync();
        }

    }
}
