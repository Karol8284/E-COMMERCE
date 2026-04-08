# E-COMMERCE

A simple e-commerce backend built with **ASP.NET Core**, **Entity Framework Core**, **PostgreSQL**, and **Docker**.

The project is focused on building a clean, scalable backend structure for an online store with support for users, products, carts, and orders.

## Tech Stack

- **Backend:** C# / ASP.NET Core Web API
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core
- **Containerization:** Docker
- **API Documentation:** Swagger / OpenAPI

## Features

- User registration and login
- Product management
- Cart management
- Order creation
- Basic validation
- Database migrations
- Dockerized development setup

## Project Goals

The main goal of this project is to create a solid backend foundation for an e-commerce platform and practice professional API design, database modeling, and application structure.

## Getting Started

### Prerequisites

- .NET SDK
- Docker
- PostgreSQL
- Git

### Run locally

1. Clone the repository
2. Configure the database connection string
3. Run migrations
4. Start the application

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## API Documentation

Swagger is available in development mode.

## Environment Variables

Example:

```env
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=ecommerce;Username=postgres;Password=your_password
```

## License

This project is licensed under the MIT License.
