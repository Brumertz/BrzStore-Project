using CarService.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace CarService.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string UnitName { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; } = string.Empty;

        // Embed the Category object as part of the Product document
        [BsonElement("Category")]
        public Category Category { get; set; } = new Category();
    }
}
