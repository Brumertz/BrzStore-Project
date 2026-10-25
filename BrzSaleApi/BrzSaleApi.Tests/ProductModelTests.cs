using System.ComponentModel.DataAnnotations;
using BrzSaleApi.Models;
using Xunit;

namespace BrzSaleApi.Tests;

public class ProductModelTests
{
    [Fact]
    public void Product_WithValidData_PassesValidation()
    {
        // Arrange
        var product = new Product
        {
            Id = "60d5ecb8b5c9c614b8e9f1a1",
            Brand = "Subaru",
            Model = "BRZ Limited",
            Year = 2024,
            Price = 31995m,
            EngineSize = 2400,
            OnSale = true,
            CategoryId = "60d5ecb8b5c9c614b8e9f1a2",
            Category = new Category { Id = "60d5ecb8b5c9c614b8e9f1a2", Brand = "Subaru" }
        };

        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(product, null, null);

        // Act
        var isValid = Validator.TryValidateObject(product, context, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData("", "Model1", "CategoryId1")]
    [InlineData("Brand1", "", "CategoryId1")]
    [InlineData("Brand1", "Model1", "")]
    public void Product_WithMissingRequiredFields_FailsValidation(string brand, string model, string categoryId)
    {
        // Arrange
        var product = new Product
        {
            Brand = brand,
            Model = model,
            CategoryId = categoryId
        };

        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(product, null, null);

        // Act
        var isValid = Validator.TryValidateObject(product, context, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.NotEmpty(validationResults);
    }

    [Fact]
    public void Product_DefaultValues_AreExpected()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.Equal(string.Empty, product.Id);
        Assert.Equal(string.Empty, product.Brand);
        Assert.Equal(string.Empty, product.Model);
        Assert.Equal(0, product.Year);
        Assert.Equal(0m, product.Price);
        Assert.False(product.OnSale);
        Assert.Equal(0, product.EngineSize);
        Assert.NotNull(product.Category);
    }
}
