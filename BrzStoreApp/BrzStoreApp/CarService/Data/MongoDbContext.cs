using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using CarService.Models;

namespace CarService.Data
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("CarService");
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
    }
}
