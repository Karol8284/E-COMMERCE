PROJECT NOTE — E-COMMERCE BACKEND

Goal:
Build a clean, scalable e-commerce backend in ASP.NET Core using C#, Entity Framework Core, PostgreSQL and Docker. The project should be simple enough to finish quickly, but structured professionally so it can later be expanded to mid-level quality.

Main idea:
Create a REST API for an online store with users, products, cart and orders. Focus first on core functionality and clean architecture. Use AI-assisted coding for boilerplate, but keep the main logic written and controlled manually.

Current scope:
- Backend only for now
- Optional frontend can be added later
- The main goal is a solid, working API with database support
- Keep the code easy to extend

Technology stack:
- C#
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker
- Swagger / OpenAPI
- JWT later if time allows

Project structure:
- Controllers/
- Models/
- DTOs/
- Data/
- Services/ (optional, if logic grows)
- Migrations/
- wwwroot/ (if needed later)

Core entities:
1. User
   - Id
   - Email
   - Password
   - FullName
   - PhoneNumber
   - Address
   - CreatedAt

2. Product
   - Id
   - Name
   - Description
   - Price
   - Stock
   - ImageUrl
   - Category
   - CreatedAt

3. CartItem
   - Id
   - UserId
   - ProductId
   - Quantity
   - AddedAt

4. Order
   - Id
   - UserId
   - CreatedAt
   - TotalPrice
   - Status
   - ShippingAddress
   - DeliveryDate

5. OrderItem
   - Id
   - OrderId
   - ProductId
   - Quantity
   - Price

DTOs to create:
- RegisterDto
- LoginDto
- AuthResponseDto
- ProductDto
- CreateProductDto
- CartItemDto
- AddToCartDto
- OrderDto
- CreateOrderDto
- OrderItemDto

Required features:
1. User registration and login
2. Product CRUD
3. Cart add / remove / clear
4. Order creation from cart or order items
5. Basic validation with DataAnnotations
6. EF Core migrations
7. Seed sample products
8. Swagger documentation
9. Docker support
10. Basic error handling

Coding rules:
- Keep the code clean and readable
- Prefer simple solutions over overengineering
- Use async/await everywhere possible
- Use DTOs instead of exposing entities directly
- Add comments only when necessary
- If a service layer is introduced, keep business logic out of controllers
- Use PascalCase for classes and properties
- Use camelCase for local variables and parameters

Implementation priorities:
1. Set up the project and database connection
2. Create models and DbContext
3. Add migrations and seed data
4. Build product endpoints
5. Build cart endpoints
6. Build order endpoints
7. Add authentication later if time allows
8. Add Swagger and Docker configuration
9. Polish README and project structure

Important decisions:
- Do not overcomplicate the first version
- Authentication can be basic at first or postponed
- Focus on working endpoints, not perfect architecture
- The project should be strong enough to later grow into a mid-level portfolio project

Expected result:
A working e-commerce API that includes:
- Database integration
- Working CRUD endpoints
- Clear folder structure
- Professional naming
- Easy future expansion

Notes for Copilot:
When generating code, prefer:
- small, focused classes
- minimal dependencies
- straightforward controller logic
- separate DTOs and entities
- practical, production-like structure
- code that is easy to understand and extend
