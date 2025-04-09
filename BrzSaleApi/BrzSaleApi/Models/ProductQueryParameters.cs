namespace BrzSaleApi.Models
{
    public class ProductQueryParameters : QueryParameters
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string Brand { get; set; } = String.Empty;
        public string Model { get; set; } = String.Empty;
    }
}
