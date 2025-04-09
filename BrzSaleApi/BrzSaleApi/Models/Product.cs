using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;


namespace BrzSaleApi.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id{ get; set; } = string.Empty;

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        public decimal Price { get; set; }

        public bool OnSale { get; set; }

        public int EngineSize { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; } = string.Empty;

        // Embed the Category object as part of the Product document
        [BsonElement("Category")]
        public Category Category { get; set; } = new Category();
    }
}
