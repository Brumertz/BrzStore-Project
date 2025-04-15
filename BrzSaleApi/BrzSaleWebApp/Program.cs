using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using BrzSaleWebApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BrzSaleWebApp.Data;
using Microsoft.EntityFrameworkCore;
using BrzSaleWebApp.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BrzSaleWebAppContextConnection") ?? throw new InvalidOperationException("Connection string 'BrzSaleWebAppContextConnection' not found.");;

builder.Services.AddDbContext<BrzSaleWebAppContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<BrzSaleWebAppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<BrzSaleWebAppContext>();

// Load MongoDB settings from appsettings.json
var mongoSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDbSettings>();

// ? Register Identity with MongoDbCore (using strongly typed config)
builder.Services.ConfigureMongoDbIdentityUserOnly<ApplicationUser,Guid>(
    new MongoDbIdentityConfiguration
    {
        MongoDbSettings = new MongoDbSettings
        {
            ConnectionString = mongoSettings.ConnectionString,
            DatabaseName = mongoSettings.DatabaseName
        },
        IdentityOptionsAction = options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.User.RequireUniqueEmail = true;
        }
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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

// Razor Pages + Identity Cookie Config
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.QueryStringApiVersionReader("api-version");
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
