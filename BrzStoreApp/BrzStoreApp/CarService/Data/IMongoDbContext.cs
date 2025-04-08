using MongoDB.Driver;
using CarService.Models;

namespace CarService.Data
{
    public interface IMongoDbContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Category> Categories { get; }
    }
}
