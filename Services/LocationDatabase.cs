using SQLite;
using HeatMapApp.Models;

namespace HeatMapApp.Services
{
    public class LocationDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public LocationDatabase(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<LocationModel>().Wait();
        }

        public Task<List<LocationModel>> GetLocationsAsync() =>
            _db.Table<LocationModel>().OrderBy(l => l.Timestamp).ToListAsync();

        public Task<int> SaveLocationAsync(LocationModel location) =>
            _db.InsertAsync(location);
    }
}
