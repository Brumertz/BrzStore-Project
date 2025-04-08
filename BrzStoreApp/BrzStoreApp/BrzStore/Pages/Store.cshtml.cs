using Microsoft.AspNetCore.Mvc.RazorPages;
using BrzStore.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace BrzStore.Pages
{
    public class StoreModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public List<Product> Products { get; set; } = new();

        public StoreModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BrzSalesApi");
        }

        public async Task OnGetAsync()
        {
            // Assuming the API endpoint returns a list of products
            Products = await _httpClient.GetFromJsonAsync<List<Product>>("api/v1.0/products") ?? new();
        }

        public async Task<IActionResult> OnPostBuyAsync(string productId)
        {
            // Handling purchase here (perhaps notify the API for the purchase)
            TempData["SuccessMessage"] = $"Product {productId} purchased!";
            return RedirectToPage("/Store");
        }
    }
}
