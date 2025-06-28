using Microsoft.Maui.Devices.Sensors;
using System;
using System.Threading.Tasks;

namespace HeatMapApp.Services
{
    public class LocationService
    {
        public async Task<Location?> GetCurrentLocationAsync()
        {
            try
            {
                return await Geolocation.Default.GetLocationAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
