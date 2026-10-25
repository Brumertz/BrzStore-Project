using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BrzSaleWebApp.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Load MongoDB settings from appsettings.json with fallback defaults
var mongoSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDbSettings>() 
    ?? new MongoDbSettings();

var connectionString = mongoSettings.ConnectionString ?? "mongodb://localhost:27017";
var databaseName = mongoSettings.DatabaseName ?? "BrzSaleApiDb";

// Configure MongoDB settings for Identity
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = connectionString;
    options.DatabaseName = databaseName;
});

// Add Identity services using ApplicationUser and MongoDB store
builder.Services.AddIdentity<ApplicationUser, MongoIdentityRole>(options =>
{
    // Password requirements configuration
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
    connectionString,
    databaseName
)
.AddDefaultTokenProviders();

// Configure authentication: preserve Identity Application Cookie as default scheme for web pages, while supporting JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    var key = Encoding.UTF8.GetBytes("YourSuperSecureJWTSecretKey");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Register fake email sender for development confirmation emails
builder.Services.AddTransient<IEmailSender, BrzSaleWebApp.Services.FakeEmailSender>();

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
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure HTTP request pipeline
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