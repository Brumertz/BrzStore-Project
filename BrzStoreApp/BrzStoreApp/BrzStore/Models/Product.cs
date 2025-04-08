using CarService.Models;

namespace BrzStore.Models
{
    public class Product
    {
        public string Id { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        public string CategoryId { get; set; } = string.Empty;

        // Embed the Category object as part of the Product document
        public Category Category { get; set; } = new Category();
    }
}
