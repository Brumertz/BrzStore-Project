using BrzStore.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Get MongoDB connection string from config
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDbConnection");

// Register MongoDB client
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));

// Register UserService via interface
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddHttpClient("BrzSalesApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7069"); // o tu base real
});
// Add Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
