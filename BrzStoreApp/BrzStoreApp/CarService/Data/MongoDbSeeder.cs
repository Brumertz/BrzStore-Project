using CarService.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Data
{
    public class MongoDbSeeder
    {
        private readonly IMongoCollection<Category> _categories;
        private readonly IMongoCollection<Product> _products;

        public MongoDbSeeder(IMongoDatabase database)
        {
            _categories = database.GetCollection<Category>("Categories");
            _products = database.GetCollection<Product>("Products");
        }

        public async Task SeedDataAsync()
        {
            if (!(await _categories.Find(_ => true).AnyAsync()))
            {
                var categories = new List<Category>
                {
                    new Category { Id = "1", Name = "Active Wear - Men" },
                    new Category { Id = "2", Name = "Active Wear - Women" },
                    new Category { Id = "3", Name = "Mineral Water" },
                    new Category { Id = "4", Name = "Publications" },
                    new Category { Id = "5", Name = "Supplements" }
                };
                await _categories.InsertManyAsync(categories);
            }

            if (!(await _products.Find(_ => true).AnyAsync()))
            {
                var products = new List<Product>
                {
                    new Product { Id = "1", CategoryId = "1", Name = "Grunge Skater Jeans", Description = "AWMGSJ", Price = 68, IsAvailable = true },
                    new Product { Id = "2", CategoryId = "1", Name = "Polo Shirt", Description = "AWMPS", Price = 35, IsAvailable = true },
                    new Product { Id = "3", CategoryId = "1", Name = "Skater Graphic T-Shirt", Description = "AWMSGT", Price = 33, IsAvailable = true },
                    new Product { Id = "4", CategoryId = "1", Name = "Slicker Jacket", Description = "AWMSJ", Price = 125, IsAvailable = true },
                    new Product { Id = "5", CategoryId = "1", Name = "Thermal Fleece Jacket", Description = "AWMTFJ", Price = 60, IsAvailable = true },
                    new Product { Id = "6", CategoryId = "1", Name = "Unisex Thermal Vest", Description = "AWMUTV", Price = 95, IsAvailable = true },
                    new Product { Id = "7", CategoryId = "1", Name = "V-Neck Pullover", Description = "AWMVNP", Price = 65, IsAvailable = true },
                    new Product { Id = "8", CategoryId = "1", Name = "V-Neck Sweater", Description = "AWMVNS", Price = 65, IsAvailable = true },
                    new Product { Id = "9", CategoryId = "1", Name = "V-Neck T-Shirt", Description = "AWMVNT", Price = 17, IsAvailable = true },
                    new Product { Id = "10", CategoryId = "2", Name = "Bamboo Thermal Ski Coat", Description = "AWWBTSC", Price = 99, IsAvailable = true },
                    new Product { Id = "11", CategoryId = "2", Name = "Cross-Back Training Tank", Description = "AWWCTT", Price = 0, IsAvailable = false },
                    new Product { Id = "12", CategoryId = "2", Name = "Grunge Skater Jeans", Description = "AWWGSJ", Price = 68, IsAvailable = true },
                    new Product { Id = "13", CategoryId = "2", Name = "Slicker Jacket", Description = "AWWSJ", Price = 125, IsAvailable = true },
                    new Product { Id = "14", CategoryId = "2", Name = "Stretchy Dance Pants", Description = "AWWSDP", Price = 55, IsAvailable = true },
                    new Product { Id = "15", CategoryId = "2", Name = "Ultra-Soft Tank Top", Description = "AWWUTT", Price = 22, IsAvailable = true },
                    new Product { Id = "16", CategoryId = "2", Name = "Unisex Thermal Vest", Description = "AWWUTV", Price = 95, IsAvailable = true },
                    new Product { Id = "17", CategoryId = "2", Name = "V-Next T-Shirt", Description = "AWWVNT", Price = 17, IsAvailable = true },
                    new Product { Id = "18", CategoryId = "3", Name = "Blueberry Mineral Water", Description = "MWB", Price = 2.8M, IsAvailable = true },
                    new Product { Id = "19", CategoryId = "3", Name = "Lemon-Lime Mineral Water", Description = "MWLL", Price = 2.8M, IsAvailable = true },
                    new Product { Id = "20", CategoryId = "3", Name = "Orange Mineral Water", Description = "MWO", Price = 2.8M, IsAvailable = true },
                    new Product { Id = "21", CategoryId = "3", Name = "Peach Mineral Water", Description = "MWP", Price = 2.8M, IsAvailable = true },
                    new Product { Id = "22", CategoryId = "3", Name = "Raspberry Mineral Water", Description = "MWR", Price = 2.8M, IsAvailable = true },
                    new Product { Id = "23", CategoryId = "3", Name = "Strawberry Mineral Water", Description = "MWS", Price = 2.8M, IsAvailable = true },
                    new Product { Id = "24", CategoryId = "4", Name = "In the Kitchen with H+ Sport", Description = "PITK", Price = 24.99M, IsAvailable = true },
                    new Product { Id = "25", CategoryId = "5", Name = "Calcium 400 IU (150 tablets)", Description = "SC400", Price = 9.99M, IsAvailable = true },
                    new Product { Id = "26", CategoryId = "5", Name = "Flaxseed Oil 100 mg (90 capsules)", Description = "SFO100", Price = 12.49M, IsAvailable = true },
                    new Product { Id = "27", CategoryId = "5", Name = "Iron 65 mg (150 caplets)", Description = "SI65", Price = 13.99M, IsAvailable = true },
                    new Product { Id = "28", CategoryId = "5", Name = "Magnesium 250 mg (100 tablets)", Description = "SM250", Price = 12.49M, IsAvailable = true },
                    new Product { Id = "29", CategoryId = "5", Name = "Multi-Vitamin (90 capsules)", Description = "SMV", Price = 9.99M, IsAvailable = true },
                    new Product { Id = "30", CategoryId = "5", Name = "Vitamin A 10,000 IU (125 caplets)", Description = "SVA", Price = 11.99M, IsAvailable = true },
                    new Product { Id = "31", CategoryId = "5", Name = "Vitamin B-Complex (100 caplets)", Description = "SVB", Price = 12.99M, IsAvailable = true },
                    new Product { Id = "32", CategoryId = "5", Name = "Vitamin C 1000 mg (100 tablets)", Description = "SVC", Price = 9.99M, IsAvailable = true },
                    new Product { Id = "33", CategoryId = "5", Name = "Vitamin D3 1000 IU (100 tablets)", Description = "SVD3", Price = 12.49M, IsAvailable = true }
                };
                await _products.InsertManyAsync(products);
            }
        }
    }
}
