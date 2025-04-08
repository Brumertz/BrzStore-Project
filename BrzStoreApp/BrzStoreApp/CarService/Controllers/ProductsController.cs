using CarService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    [ApiController]
    public class ProductsV1Controller : ControllerBase
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductsV1Controller(IMongoDatabase database)
        {
            _productCollection = database.GetCollection<Product>("Products");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            var filter = Builders<Product>.Filter.Empty; // Start with no filter

            if (queryParameters.MinPrice != null)
            {
                filter &= Builders<Product>.Filter.Gte(p => p.Price, queryParameters.MinPrice.Value);
            }

            if (queryParameters.MaxPrice != null)
            {
                filter &= Builders<Product>.Filter.Lte(p => p.Price, queryParameters.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.Sku, queryParameters.Sku);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                filter &= Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(queryParameters.Name, "i"));
            }

            var products = await _productCollection.Find(filter)
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Limit(queryParameters.Size)
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(string id)
        {
            // Convert string id to ObjectId
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid product ID format.");
            }

            var product = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(string id, Product product)
        {
            // Convert string id to ObjectId
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid product ID format.");
            }

            product.Id = id; // Ensure that the product has the correct ObjectId as string
            var result = await _productCollection.ReplaceOneAsync(p => p.Id == id, product);

            if (result.MatchedCount == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            // Convert string id to ObjectId
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid product ID format.");
            }

            var result = await _productCollection.DeleteOneAsync(p => p.Id == id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] string[] ids)
        {
            var objectIds = ids.Select(id => ObjectId.Parse(id)).ToList();
            var result = await _productCollection.DeleteManyAsync(p => objectIds.Contains(ObjectId.Parse(p.Id)));
            return Ok(result.DeletedCount);
        }
    }
    [ApiVersion("2.0")]
    [Route("products")]
    [ApiController]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductsV2Controller(IMongoDatabase database)
        {
            _productCollection = database.GetCollection<Product>("Products");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.IsAvailable, true);

            if (queryParameters.MinPrice != null)
            {
                filter &= Builders<Product>.Filter.Gte(p => p.Price, queryParameters.MinPrice.Value);
            }

            if (queryParameters.MaxPrice != null)
            {
                filter &= Builders<Product>.Filter.Lte(p => p.Price, queryParameters.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.Sku, queryParameters.Sku);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                filter &= Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(queryParameters.Name, "i"));
            }

            var products = await _productCollection.Find(filter)
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Limit(queryParameters.Size)
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(string id)
        {
            // Convert string id to ObjectId
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid product ID format.");
            }

            var product = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(string id, Product product)
        {
            // Convert string id to ObjectId
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid product ID format.");
            }

            product.Id = objectId.ToString(); // Ensure that the product has the correct ObjectId as string
            var result = await _productCollection.ReplaceOneAsync(p => p.Id == id, product);

            if (result.MatchedCount == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            // Convert string id to ObjectId
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid product ID format.");
            }
            var result = await _productCollection.DeleteOneAsync(p => p.Id == id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] string[] ids)
        {
            var objectIds = ids.Select(id => ObjectId.Parse(id)).ToList();
            var result = await _productCollection.DeleteManyAsync(p => objectIds.Contains(ObjectId.Parse(p.Id)));
            return Ok(result.DeletedCount);
        }
    }
}
