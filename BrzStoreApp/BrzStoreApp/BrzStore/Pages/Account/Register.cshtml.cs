// File: Register.cshtml.cs
using BrzStore.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;


namespace BrzStore.Pages.Account
{


public class RegisterModel : PageModel
{
    private readonly IUserService _userService;

    public RegisterModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public string Email { get; set; }
    [BindProperty]
    public string Password { get; set; }
    [BindProperty]
    public string ConfirmPassword { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Password != ConfirmPassword)
        {
            ModelState.AddModelError("", "Passwords do not match.");
            return Page();
        }

        var result = await _userService.RegisterAsync(Email, Password);
        if (result)
        {
            return RedirectToPage("/Account/Login"); // Redirect to login after successful registration
        }

        ModelState.AddModelError("", "Registration failed. Please try again.");
        return Page();
    }
}
}