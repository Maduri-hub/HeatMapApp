using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeatMapApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            DrawHeatmap();
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            MyMap.MapElements.Clear();
            DrawHeatmap();
        }

        private void DrawHeatmap()
        {
            var pathPoints = GetInterpolatedPoints();

            foreach (var point in pathPoints)
            {
                var circle = new Circle
                {
                    Center = point,
                    Radius = new Distance(80),
                    StrokeColor = Colors.Transparent,
                    FillColor = Colors.Blue.WithAlpha(0.6f)
                };
                MyMap.MapElements.Add(circle);
            }

            // Center map between Suwanee and Decatur
            var center = new Location(33.9, -84.0);
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(25)));
        }

        private List<Location> GetInterpolatedPoints()
        {
            var path = new List<Location>
            {
                new Location(34.0515, -84.0713),  // Suwanee
                new Location(34.0029, -84.1446),  // Duluth
                new Location(33.7748, -84.2963),  // Decatur
            };

            var interpolated = new List<Location>();
            const int segments = 30;

            for (int i = 0; i < path.Count - 1; i++)
            {
                var start = path[i];
                var end = path[i + 1];

                for (int j = 0; j <= segments; j++)
                {
                    var lat = start.Latitude + (end.Latitude - start.Latitude) * j / segments;
                    var lng = start.Longitude + (end.Longitude - start.Longitude) * j / segments;
                    interpolated.Add(new Location(lat, lng));
                }
            }

            return interpolated;
        }
    }
}
