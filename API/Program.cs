using API.Filters;
using API.Middleware;
using CORE.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

//builder.Services.AddScoped<IProduct>



var app = builder.Build();

// Apply migrations and seed data automatically
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Create database schema from models
        await dbContext.Database.EnsureCreatedAsync();

        dbContext.SeedData();
        Console.WriteLine("✓ Database initialized successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ Database initialization failed: {ex.Message}");
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
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
