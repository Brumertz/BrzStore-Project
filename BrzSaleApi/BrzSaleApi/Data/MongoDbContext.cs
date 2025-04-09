using MongoDB.Driver;
using BrzSaleApi.Models;
using Microsoft.Extensions.Options;
using BrzSaleApi.Settings;

namespace BrzSaleApi.Data
{
    public class MongoDbContext(IMongoClient mongoClient, IOptions<MongoDBSettings> settings) : IMongoDbContext
    {
        private readonly IMongoDatabase _database = mongoClient.GetDatabase(settings.Value.DatabaseName);

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
    }
}
