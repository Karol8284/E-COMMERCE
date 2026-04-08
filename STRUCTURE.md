# Project Structure

```text
E-COMMERCE/
├── Controllers/
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── CartController.cs
│   └── OrdersController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── DTOs/
│   ├── Auth/
│   │   ├── LoginDto.cs
│   │   ├── RegisterDto.cs
│   │   └── AuthResponseDto.cs
│   │
│   ├── Product/
│   │   ├── ProductDto.cs
│   │   └── CreateProductDto.cs
│   │
│   ├── Cart/
│   │   ├── CartItemDto.cs
│   │   └── AddToCartDto.cs
│   │
│   └── Order/
│       ├── CreateOrderDto.cs
│       ├── OrderDto.cs
│       └── OrderItemDto.cs
│
├── Models/
│   ├── User.cs
│   ├── Product.cs
│   ├── CartItem.cs
│   ├── Order.cs
│   └── OrderItem.cs
│
├── Migrations/
│   └── (EF Core migrations)
│
├── Properties/
│   └── launchSettings.json
│
├── wwwroot/
│   └── (static files if needed)
│
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
├── Dockerfile
├── docker-compose.yml
├── README.md
├── PLAN.md
├── STRUCTURE.md
└── LICENSE
```

## Folder Responsibilities

### Controllers
Contains API endpoints and request handling.

### Data
Contains database context and configuration.

### DTOs
Contains request and response models used by the API.

### Models
Contains domain entities mapped to database tables.

### Migrations
Contains EF Core migration files.

### Docker files
Contains containerization configuration for local and production use.
