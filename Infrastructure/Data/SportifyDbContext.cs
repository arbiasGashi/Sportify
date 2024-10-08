using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Data;

public class SportifyDbContext : DbContext
{
    private readonly ILogger<SportifyDbContext> _logger;

    public SportifyDbContext(DbContextOptions<SportifyDbContext> options, ILogger<SportifyDbContext> logger) : base(options)
    {
        _logger = logger;
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductBrand> ProductBrands { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Load data from JSON files
        var productBrands = LoadSeedData<ProductBrand>("SeedData/brands.json", _logger);
        var productTypes = LoadSeedData<ProductType>("SeedData/types.json", _logger);
        var products = LoadSeedData<Product>("SeedData/products.json", _logger);

        // Assign negative IDs for seed data
        int brandId = -1;
        foreach (var brand in productBrands)
        {
            brand.Id = brandId--;
        }

        int typeId = -1;
        foreach (var type in productTypes)
        {
            type.Id = typeId--;
        }

        int productId = -1;
        foreach (var product in products)
        {
            product.Id = productId--;
        }

        modelBuilder.Entity<ProductBrand>().HasData(productBrands);
        modelBuilder.Entity<ProductType>().HasData(productTypes);
        modelBuilder.Entity<Product>().HasData(products);
    }

    private List<T> LoadSeedData<T>(string filePath, ILogger logger) where T : class
    {
        // Get the current base path
        var basePath = AppContext.BaseDirectory;

        // Adjust the path relative to the Infrastructure folder
        var fullPath = Path.Combine(basePath, "Data", filePath);

        if (!File.Exists(fullPath))
        {
            logger.LogError($"File not found: {fullPath}");
            throw new FileNotFoundException($"Could not find the JSON file at path: {fullPath}");
        }

        var jsonData = File.ReadAllText(fullPath);
        var result = JsonSerializer.Deserialize<List<T>>(jsonData);

        if (result == null)
        {
            logger.LogError($"Failed to deserialize JSON data from {filePath}");
            logger.LogDebug($"Content of {filePath}: {jsonData}");

            throw new InvalidOperationException($"Failed to deserialize JSON data from {filePath}");
        }

        return result;
    }
}
