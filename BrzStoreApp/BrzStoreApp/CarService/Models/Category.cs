using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace CarService.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Converts string to ObjectId
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
