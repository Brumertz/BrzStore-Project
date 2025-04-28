using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BrzSaleWebApp.Models; // Ensure this namespace contains your ApplicationUser class

var builder = WebApplication.CreateBuilder(args);

// Load MongoDB settings from appsettings.json
var mongoSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDbSettings>();

// Configure MongoDB settings for Identity
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = mongoSettings.ConnectionString;
    options.DatabaseName = mongoSettings.DatabaseName;
});

// Add Identity services using ApplicationUser. If you are not using roles,
// replace MongoIdentityRole with IdentityRole (from Microsoft.AspNetCore.Identity)
// and adjust the AddMongoDbStores accordingly.
builder.Services.AddIdentity<ApplicationUser, MongoIdentityRole>(options =>
{
    // Password settings configuration
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;

    // User settings configuration
    options.User.RequireUniqueEmail = true;
})
.AddMongoDbStores<ApplicationUser, MongoIdentityRole, Guid>(
    mongoSettings.ConnectionString,
    mongoSettings.DatabaseName
)
.AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes("YourSuperSecureJWTSecretKey"); // Consider moving this to configuration
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Configure Razor Pages and Identity Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Configure API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.QueryStringApiVersionReader("api-version");
});

// Configure API Version Explorer for discoverability
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();  // Enable authorization middleware

app.MapRazorPages();
app.MapControllers();

app.Run();