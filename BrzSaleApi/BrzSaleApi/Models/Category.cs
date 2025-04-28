using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

public class Category
{
    [BsonId]
     // Converts string to ObjectId
    public string Id { get; set; } = string.Empty;

    [Required]
    public string Brand { get; set; } = string.Empty;
}
