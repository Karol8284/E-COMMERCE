using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure DbContext - use DefaultConnection from secrets or environment
var connectionString = builder.Configuration["DefaultConnection"]
    ?? builder.Configuration["DbConnectionString"]
    ?? throw new InvalidOperationException("Connection string (DefaultConnection or DbConnectionString) not found in configuration");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Swagger
builder.Services.AddSwaggerGen();

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
