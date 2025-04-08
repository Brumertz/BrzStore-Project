using CarService.Models;
using MongoDB.Driver;

public class ProductService
{
    private readonly IMongoCollection<Product> _productCollection;
    private readonly IMongoCollection<Category> _categoryCollection;

    public ProductService(IMongoDatabase database)
    {
        _productCollection = database.GetCollection<Product>("Products");
        _categoryCollection = database.GetCollection<Category>("Categories");
    }

    public async Task<Product?> GetProductByIdAsync(string productId)
    {
        // Retrieve the product
        var product = await _productCollection.Find(p => p.Id == productId).FirstOrDefaultAsync();

        if (product != null)
        {
            // Manually retrieve the Category based on CategoryId
            var category = await _categoryCollection.Find(c => c.Id == product.CategoryId).FirstOrDefaultAsync();
            product.Category = category;  // Populate the Category property
        }

        return product;
    }
}
