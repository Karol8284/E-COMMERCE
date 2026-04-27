using API.Filters;
using API.Middleware;
using CORE.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog structured logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/app-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 7
    )
    .CreateLogger();

try
{
    Log.Information("🚀 Application starting up...");
    
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers()
        .AddFluentValidation(config =>
        {
            config.RegisterValidatorsFromAssemblyContaining<Program>();
            config.DisableDataAnnotationsValidation = false;
        });
    builder.Services.AddOpenApi();

    // Configure DbContext
    var connectionString = builder.Configuration["DefaultConnection"]
        ?? builder.Configuration["DbConnectionString"]
        ?? throw new InvalidOperationException("Connection string not found");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));

    // Add Swagger
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<ValidationFilterAttribute>();

    var app = builder.Build();

    Log.Information("📊 Configuring application middleware...");

    // Apply migrations and seed data automatically
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Create database schema from models
            await dbContext.Database.EnsureCreatedAsync();

            dbContext.SeedData();
            Log.Information("✓ Database initialized successfully!");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "✗ Database initialization failed");
            // Don't crash the application if database initialization fails
        }
    }

    // Configure the HTTP request pipeline
    // Add exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
        Log.Information("📖 Swagger UI available at /swagger");
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("🌐 Application listening on configured ports");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "💥 Application terminated unexpectedly");
}
finally
{
    Log.Information("🛑 Application shutting down...");
    await Log.CloseAndFlushAsync();
}
