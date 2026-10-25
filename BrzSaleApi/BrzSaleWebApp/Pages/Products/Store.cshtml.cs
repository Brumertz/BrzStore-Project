using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using BrzSaleWebApp.Models;
using BrzSaleApi.Models;
using Microsoft.AspNetCore.Identity;

namespace BrzSaleWebApp.Pages.Products
{
    public class StoreModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public List<Product> Products { get; set; } = new List<Product>();

        // Expose authentication state for the Razor view
        public bool IsAuthenticated { get; private set; }

        public StoreModel(IHttpClientFactory httpClientFactory, SignInManager<ApplicationUser> signInManager)
        {
            _httpClient = httpClientFactory.CreateClient();
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
            // Check if the current user is logged in
            IsAuthenticated = _signInManager.IsSignedIn(User);

            // Call the products API endpoint
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
                // Log error when API is unreachable
                Console.WriteLine("Error fetching products: " + ex.Message);
            }
        }
    }
}
