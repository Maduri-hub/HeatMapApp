using HeatMapApp.Models;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HeatMapApp.Services
{
    public class LocationDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public LocationDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<LocationModel>().Wait();
        }

        public Task<List<LocationModel>> GetLocationsAsync()
        {
            return _database.Table<LocationModel>().OrderBy(x => x.Timestamp).ToListAsync();
        }

        public Task<int> SaveLocationAsync(LocationModel location)
        {
            return _database.InsertAsync(location);
        }

        public Task<int> DeleteAllAsync()
        {
            return _database.DeleteAllAsync<LocationModel>();
        }
    }
}
