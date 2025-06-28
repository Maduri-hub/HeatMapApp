using Microsoft.Maui;
using Microsoft.Maui.Controls;
using HeatMapApp.Services;

namespace HeatMapApp
{
    public partial class App : Application
    {
        public App(LocationService locationService, LocationDatabase database)
        {
            InitializeComponent();
            MainPage = new MainPage(locationService, database);
        }
    }
}


