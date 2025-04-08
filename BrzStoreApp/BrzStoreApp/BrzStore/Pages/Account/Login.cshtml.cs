// File: Login.cshtml.cs
using BrzStore.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;



namespace BrzStore.Pages.Account
{

public class LoginModel : PageModel
{
    private readonly IUserService _userService;

    public LoginModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            ModelState.AddModelError("", "Please enter both email and password.");
            return Page();
        }

        var result = await _userService.LoginAsync(Email, Password);
        if (result)
        {
            // Redirect to the store page after successful login
            return RedirectToPage("/Store");
        }

        // If login fails, show an error message
        ModelState.AddModelError("", "Invalid login attempt.");
        return Page();
    }
}
}