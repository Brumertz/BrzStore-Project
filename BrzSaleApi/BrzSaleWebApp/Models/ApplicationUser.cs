using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;


namespace BrzSaleWebApp.Models
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid> { }
}