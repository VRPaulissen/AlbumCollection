using AlbumCollection.Core.Repositories;
using AlbumCollection.Core.Services;
using AlbumCollection.Infrastructure.Data;
using AlbumCollection.Infrastructure.Repositories;
using AlbumCollection.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework Core to use SQLite Server
builder.Services.AddDbContext<AlbumContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register the album repository abstraction with its EF Core implementation
// IAlbumRepository requests will resolve to EfAlbumRepository instances
builder.Services.AddScoped<IAlbumRepository, EfAlbumRepository>();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AlbumCollection API",
        Version = "v1",
        Description = "CRUD endpoints for your album closet"
    });
});

// Add support for controllers (enables attribute-based routing and API endpoints)
builder.Services.AddControllers();
builder.Services.Configure<DiscogsOptions>(builder.Configuration.GetSection("Discogs"));

builder.Services.AddHttpClient<IDiscogsService, DiscogsService>((sp, client) =>
{
    var opts = sp.GetRequiredService<IOptions<DiscogsOptions>>().Value;
    client.BaseAddress = new Uri(opts.BaseUrl);

    // Discogs requires a User-Agent header
    client.DefaultRequestHeaders.UserAgent.ParseAdd("AlbumCollectionApp/1.0");

    // Use your personal access token for Authorization
    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Discogs", $"token={opts.PersonalAccessToken}");
});

// Build the application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                           
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlbumCollection API v1");
    });
}

app.UseHttpsRedirection();      
app.UseAuthorization();        
app.MapControllers();

// Start listening for incoming HTTP requests
app.Run();