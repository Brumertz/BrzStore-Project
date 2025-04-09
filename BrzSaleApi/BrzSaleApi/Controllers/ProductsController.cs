using BrzSaleApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using BrzSaleApi.Data;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Products")]
[ApiController]
public class ProductsV1Controller : ControllerBase
{
    private readonly IMongoCollection<Product> _productCollection;

    // Constructor injection of IMongoDbContext
    public ProductsV1Controller(IMongoDbContext context)
    {
        _productCollection = context.Products;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
    {
        var filter = Builders<Product>.Filter.Empty;

        if (queryParameters.MinPrice != null)
            filter &= Builders<Product>.Filter.Gte(p => p.Price, queryParameters.MinPrice.Value);

        if (queryParameters.MaxPrice != null)
            filter &= Builders<Product>.Filter.Lte(p => p.Price, queryParameters.MaxPrice.Value);

        if (!string.IsNullOrEmpty(queryParameters.Model))
            filter &= Builders<Product>.Filter.Eq(p => p.Model, queryParameters.Model);

        if (!string.IsNullOrEmpty(queryParameters.Brand))
            filter &= Builders<Product>.Filter.Regex(p => p.Brand, new BsonRegularExpression(queryParameters.Brand, "i"));

        var products = await _productCollection.Find(filter)
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Limit(queryParameters.Size)
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid product ID format.");

        var product = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutProduct(string id, Product product)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid product ID format.");

        product.Id = id;
        var result = await _productCollection.ReplaceOneAsync(p => p.Id == id, product);
        return result.MatchedCount == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid product ID format.");

        var result = await _productCollection.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount == 0 ? NotFound() : NoContent();
    }

    [HttpPost("Delete")]
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

    // Constructor injection of IMongoDbContext
    public ProductsV2Controller(IMongoDbContext context)
    {
        _productCollection = context.Products;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.OnSale, true);

        if (queryParameters.MinPrice != null)
            filter &= Builders<Product>.Filter.Gte(p => p.Price, queryParameters.MinPrice.Value);

        if (queryParameters.MaxPrice != null)
            filter &= Builders<Product>.Filter.Lte(p => p.Price, queryParameters.MaxPrice.Value);

        if (!string.IsNullOrEmpty(queryParameters.Model))
            filter &= Builders<Product>.Filter.Eq(p => p.Model, queryParameters.Model);

        if (!string.IsNullOrEmpty(queryParameters.Brand))
            filter &= Builders<Product>.Filter.Regex(p => p.Brand, new BsonRegularExpression(queryParameters.Brand, "i"));

        var products = await _productCollection.Find(filter)
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Limit(queryParameters.Size)
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid product ID format.");

        var product = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutProduct(string id, Product product)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid product ID format.");

        product.Id = id;
        var result = await _productCollection.ReplaceOneAsync(p => p.Id == id, product);
        return result.MatchedCount == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid product ID format.");

        var result = await _productCollection.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount == 0 ? NotFound() : NoContent();
    }

    [HttpPost("Delete")]
    public async Task<ActionResult> DeleteMultiple([FromQuery] string[] ids)
    {
        var objectIds = ids.Select(id => ObjectId.Parse(id)).ToList();
        var result = await _productCollection.DeleteManyAsync(p => objectIds.Contains(ObjectId.Parse(p.Id)));
        return Ok(result.DeletedCount);
    }
}
