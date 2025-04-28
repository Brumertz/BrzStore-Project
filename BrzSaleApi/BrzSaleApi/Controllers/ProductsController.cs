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


        // 🛠️ Dynamic Sorting based on QueryParameters
        var sortBuilder = Builders<Product>.Sort;
        var sort = queryParameters.SortOrder == "desc"
            ? sortBuilder.Descending(queryParameters.SortBy)
            : sortBuilder.Ascending(queryParameters.SortBy);

        var products = await _productCollection.Find(filter)
            .Sort(sort)  // ✨ Added Sorting
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
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {

        if (product == null)
        {
            return BadRequest("Product cannot be null.");
        }

        // Validate CategoryId before inserting
        if (!ObjectId.TryParse(product.CategoryId, out _))
        {
            return BadRequest("Invalid CategoryId. It must be a 24-digit hex string.");
        }

        await _productCollection.InsertOneAsync(product);

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutProduct(string id, Product product)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid product ID format.");

        if (id != product.Id)
            return BadRequest("ID mismatch between URL and body.");

        var existingProduct = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (existingProduct == null)
        {
            return NotFound();
        }

        // Here we update the existing record
        var updateResult = await _productCollection.ReplaceOneAsync(p => p.Id == id, product);

        if (updateResult.MatchedCount == 0)
            return NotFound();

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid product ID format.");

        var result = await _productCollection.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult> DeleteMultiple([FromQuery] string[] ids)
    {
        var objectIds = ids.Select(id => ObjectId.Parse(id)).ToList();
        var result = await _productCollection.DeleteManyAsync(p => objectIds.Contains(ObjectId.Parse(p.Id)));
        return Ok(result.DeletedCount);
    }
}

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/products")]
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
        // Start by filtering only products that are On Sale
        var filter = Builders<Product>.Filter.Eq(p => p.OnSale, true);

        // Apply a minimum price filter if provided
        if (queryParameters.MinPrice.HasValue)
        {
            filter &= Builders<Product>.Filter.Gte(p => p.Price, queryParameters.MinPrice.Value);
        }

        // Apply a maximum price filter if provided
        if (queryParameters.MaxPrice.HasValue)
        {
            filter &= Builders<Product>.Filter.Lte(p => p.Price, queryParameters.MaxPrice.Value);
        }

        // Apply a model filter if provided
        if (!string.IsNullOrEmpty(queryParameters.Model))
        {
            filter &= Builders<Product>.Filter.Eq(p => p.Model, queryParameters.Model);
        }

        // Apply a brand filter using case-insensitive regular expression if provided
        if (!string.IsNullOrEmpty(queryParameters.Brand))
        {
            filter &= Builders<Product>.Filter.Regex(p => p.Brand, new MongoDB.Bson.BsonRegularExpression(queryParameters.Brand, "i"));
        }

        // Determine sorting based on SortBy and SortOrder parameters
        var sortBuilder = Builders<Product>.Sort;
        var sort = queryParameters.SortOrder.ToLower() == "desc"
            ? sortBuilder.Descending(queryParameters.SortBy)
            : sortBuilder.Ascending(queryParameters.SortBy);

        // Execute the query with pagination and sorting
        var products = await _productCollection.Find(filter)
            .Sort(sort)
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Limit(queryParameters.Size)
            .ToListAsync();

        // Return the list of products
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

        if (id != product.Id)
            return BadRequest("ID mismatch between URL and body.");

        var existingProduct = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (existingProduct == null)
        {
            return NotFound();
        }

        // Here we update the existing record
        var updateResult = await _productCollection.ReplaceOneAsync(p => p.Id == id, product);

        if (updateResult.MatchedCount == 0)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid product ID format.");

        var result = await _productCollection.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult> DeleteMultiple([FromQuery] string[] ids)
    {
        var objectIds = ids.Select(id => ObjectId.Parse(id)).ToList();
        var result = await _productCollection.DeleteManyAsync(p => objectIds.Contains(ObjectId.Parse(p.Id)));
        return Ok(result.DeletedCount);
    }
}
