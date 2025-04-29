using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using BrzSaleWebApp.Models;
using BrzSaleApi.Models; // Make sure you have Product class here


namespace BrzSaleWebApp.Pages.Products
{
    public class StoreModel : PageModel
{
    private readonly HttpClient _httpClient;

    public List<Product> Products { get; set; } = new List<Product>();

    public StoreModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task OnGetAsync()
    {
        // Call your API endpoint
        var apiUrl = "https://localhost:7069/api/v1.0/Products";

        try
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(apiUrl);
            if (products != null)
                {
                Products = products;
                }
            }
            catch (Exception ex)
            {
            // Log error (optional)
            Console.WriteLine("Error fetching products: " + ex.Message);
            }
        }
    }
}
