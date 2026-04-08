# Project Plan

## Phase 1 — Setup
- Create ASP.NET Core Web API project
- Configure PostgreSQL and Entity Framework Core
- Add Docker support
- Prepare initial folder structure

## Phase 2 — Domain Models
- Create core entities:
  - User
  - Product
  - CartItem
  - Order
  - OrderItem
- Define relationships between entities
- Add validations and constraints

## Phase 3 — Database
- Configure `DbContext`
- Create first migration
- Seed example data
- Verify database schema

## Phase 4 — API Layer
- Implement authentication endpoints
- Implement product endpoints
- Implement cart endpoints
- Implement order endpoints
- Add DTOs and mapping logic

## Phase 5 — Business Logic
- Add cart handling
- Add order creation logic
- Add stock validation
- Add basic error handling

## Phase 6 — Quality Improvements
- Add Swagger documentation
- Add logging
- Add tests
- Improve validation and code structure

## Phase 7 — Deployment
- Prepare Docker Compose configuration
- Test application in containerized environment
- Prepare for production deployment

## Future Improvements
- JWT authentication
- Role-based access control
- Payment integration
- Email notifications
- Admin panel
- Product categories and filters
- Wishlist functionality
- Order status tracking
