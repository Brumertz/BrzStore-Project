using MongoDB.Driver;
using BrzSaleApi.Models;

namespace BrzSaleApi.Data
{
    public interface IMongoDbContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Category> Categories { get; }
    }
}
