using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Sportify;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<SportifyDbContext>(
            opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SportifyDb")));
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        });


        var app = builder.Build();

        app.UseCors("AllowAll");

        // Get the logger from the DI container
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        // Configure the HTTP request pipeline and apply DB migrations if necessary
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            await EnsureDatabaseIsMigrated(app.Services, logger);
        }

        app.UseAuthorization();
        app.MapControllers();
        await app.RunAsync();
    }

    static async Task EnsureDatabaseIsMigrated(IServiceProvider services, ILogger<Program> logger)
    {
        try
        {
            using var scope = services.CreateScope();
            var ctx = scope.ServiceProvider.GetService<SportifyDbContext>();

            if (ctx is not null)
            {
                await ctx.Database.MigrateAsync();
                logger.LogInformation("Database migration completed successfully.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database migration.");
            throw;
        }
    }
}
