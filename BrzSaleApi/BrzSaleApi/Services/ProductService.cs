using BrzSaleApi.Data;
using BrzSaleApi.Models;
using MongoDB.Driver;

public class ProductService
{
    private readonly IMongoDbContext _context;

    public ProductService(IMongoDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(string productId)
    {
        var product = await _context.Products.Find(p => p.Id == productId).FirstOrDefaultAsync();

        if (product != null)
        {
            var category = await _context.Categories.Find(c => c.Id == product.CategoryId).FirstOrDefaultAsync();
            product.Category = category;
        }

        return product;
    }
}
