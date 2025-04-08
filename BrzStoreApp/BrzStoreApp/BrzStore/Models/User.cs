// File: User.cs (in the BrzStore.Models namespace)
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BrzStore.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Email")]
        public string? Email { get; set; }

        [BsonElement("PasswordHash")]
        public string? Password { get; set; }
    }
}
