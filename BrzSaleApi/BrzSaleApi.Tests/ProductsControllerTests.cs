using BrzSaleApi.Data;
using BrzSaleApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BrzSaleApi.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IMongoDbContext> _mockContext;
    private readonly Mock<IMongoCollection<Product>> _mockCollection;
    private readonly ProductsV1Controller _controller;

    public ProductsControllerTests()
    {
        _mockContext = new Mock<IMongoDbContext>();
        _mockCollection = new Mock<IMongoCollection<Product>>();
        _mockContext.Setup(c => c.Products).Returns(_mockCollection.Object);

        _controller = new ProductsV1Controller(_mockContext.Object);
    }

    [Fact]
    public async Task PostProduct_WithNullProduct_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.PostProduct(null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Product cannot be null.", badRequestResult.Value);
    }

    [Fact]
    public async Task PostProduct_WithInvalidCategoryId_ReturnsBadRequest()
    {
        // Arrange: CategoryId is not a valid 24-hex string
        var product = new Product
        {
            Brand = "Toyota",
            Model = "GR86",
            CategoryId = "invalid-category-id"
        };

        // Act
        var result = await _controller.PostProduct(product);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid CategoryId. It must be a 24-digit hex string.", badRequestResult.Value);
    }

    [Fact]
    public async Task GetProduct_WithInvalidIdFormat_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetProduct("invalid-id");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid product ID format.", badRequestResult.Value);
    }

    [Fact]
    public async Task PutProduct_WithInvalidIdFormat_ReturnsBadRequest()
    {
        // Arrange
        var product = new Product { Id = "invalid-id" };

        // Act
        var result = await _controller.PutProduct("invalid-id", product);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid product ID format.", badRequestResult.Value);
    }

    [Fact]
    public async Task PutProduct_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        const string urlId = "60d5ecb8b5c9c614b8e9f1a1";
        const string bodyId = "60d5ecb8b5c9c614b8e9f1a2";
        var product = new Product { Id = bodyId };

        // Act
        var result = await _controller.PutProduct(urlId, product);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID mismatch between URL and body.", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteProduct_WithInvalidIdFormat_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.DeleteProduct("invalid-id");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid product ID format.", badRequestResult.Value);
    }
}
