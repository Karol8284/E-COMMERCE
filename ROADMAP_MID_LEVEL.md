# 🎯 Roadmap do MID-Level (Backend) - 2-3 tygodnie

## 📊 Ocena Obecnego Stanu

### ✅ MASZ (Fazy 1-4):
- ASP.NET Core 9 Web API
- PostgreSQL + EF Core
- Docker Compose
- 5 Controllerów (Auth, Cart, Order, Product, User)
- Domain Models (User, Product, Cart, Order)
- Seed Data

### ⚠️ BRAKUJE LUB WYMAGA POPRAWY (Fazy 5-6):

#### **KRYTYCZNE** (musisz mieć):
1. [ ] **Exception Handling** (middleware, global error handling)
2. [ ] **Validation** (FluentValidation lub Data Annotations)
3. [ ] **Logging** (Serilog)
4. [ ] **Unit Tests** (xUnit + Moq)
5. [ ] **API Documentation** (Swagger - już masz, ale bez opisu)

#### **WYSOKIEJ WAGI** (będziesz potrzebować):
6. [ ] **JWT Authentication** (poprawnie, z refresh tokens)
7. [ ] **Authorization** (Role-based, nie tylko checks)
8. [ ] **DTOs + Mapping** (AutoMapper)
9. [ ] **Business Logic Layer** (Service layer)
10. [ ] **Repository Pattern** (jeśli go nie masz)

#### **DODATKOWE** (dla MID):
11. [ ] **Pagination** (na listach)
12. [ ] **Filtering & Sorting** (produkty, orders)
13. [ ] **Caching** (Redis lub In-Memory)
14. [ ] **API Versioning** (v1, v2...)
15. [ ] **Rate Limiting** (aby chronić API)

---

## 🚀 PLAN DZIAŁANIA - 3 TYGODNIE

### **TYDZIEŃ 1: Fundamenty** (3-4 dni)

#### Dzień 1-2: Exception Handling + Validation
```bash
Priority: 🔴 CRITICAL
Time: 4-5 godzin
```

**Co robić:**
1. Dodaj Middleware do globalnego error handling
2. Ustandaryzuj response errors
3. Dodaj FluentValidation do DTOs
4. Implementuj validation pipeline

**Pliki do zmiany:**
- API/Middleware/ExceptionHandlingMiddleware.cs (NEW)
- API/DTOs/* (add validation)
- API/Program.cs (register middleware)

**Test:**
```powershell
# Prześlij invalid data do API - powinieneś dostać ustandaryzowany error
curl -X POST http://localhost:9080/api/users/register \
  -H "Content-Type: application/json" \
  -d '{"email": "invalid"}'
```

---

#### Dzień 3-4: Logging + Structured Logging
```bash
Priority: 🔴 CRITICAL
Time: 3-4 godziny
```

**Co robić:**
1. Zainstaluj Serilog
2. Dodaj structured logging (request/response)
3. Log do pliku + console
4. Dodaj correlation IDs

**Instalacja:**
```powershell
dotnet add API package Serilog
dotnet add API package Serilog.Sinks.Console
dotnet add API package Serilog.Sinks.File
dotnet add API package Serilog.Sinks.Async
dotnet add API package Serilog.Extensions.Hosting
dotnet add API package Serilog.Extensions.Logging
```

---

### **TYDZIEŃ 1-2: Bezpieczeństwo** (3-4 dni)

#### Dzień 5-6: JWT + Proper Auth
```bash
Priority: 🔴 CRITICAL
Time: 4-5 godzin
```

**Co robić:**
1. Popraw JWT implementation (issuer, audience, expiry)
2. Dodaj Refresh Tokens
3. Implementuj Token Revocation
4. Secure password hashing (BCrypt properly)

**Kod:**
```csharp
// JWT Service - NEW
public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

// Auth Controller - UPDATE
[HttpPost("refresh")]
public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
{
    // Implement token refresh
}
```

---

#### Dzień 7: Authorization (Role-based)
```bash
Priority: 🔴 CRITICAL
Time: 3-4 godziny
```

**Co robić:**
1. Implementuj Role-Based Access Control (RBAC)
2. Dodaj Policy-based authorization
3. Secure endpoints z [Authorize(Roles = "Admin")]
4. Test z różnymi rolami

---

### **TYDZIEŃ 2: Architecture** (3-4 dni)

#### Dzień 8-10: Service Layer + Repository
```bash
Priority: 🟠 HIGH
Time: 6-8 godzin
```

**Co robić:**
1. Wydziel Business Logic do Services
2. Implementuj Repository Pattern
3. Dependency Injection setup
4. Unit of Work pattern (opcjonalnie)

**Struktura:**
```
Infrastructure/
├── Repositories/
│   ├── IRepository.cs (generic)
│   ├── UserRepository.cs
│   ├── ProductRepository.cs
│   └── OrderRepository.cs
└── Services/
    ├── IOrderService.cs
    ├── OrderService.cs
    ├── ICartService.cs
    └── CartService.cs
```

---

### **TYDZIEŃ 2-3: Testing** (3-4 dni)

#### Dzień 11-12: Unit Tests
```bash
Priority: 🟠 HIGH
Time: 5-6 godzin
```

**Co robić:**
1. Zainstaluj xUnit + Moq
2. Napisz testy dla Services
3. Mock DbContext
4. 70%+ code coverage na business logic

**Instalacja:**
```powershell
dotnet add Test package xunit
dotnet add Test package xunit.runner.visualstudio
dotnet add Test package Moq
dotnet add Test package FluentAssertions
```

**Przykład testu:**
```csharp
[Fact]
public async Task CreateOrder_WithValidData_ReturnsOrderId()
{
    // Arrange
    var service = new OrderService(mockRepository.Object);
    var request = new CreateOrderRequest { /* ... */ };
    
    // Act
    var result = await service.CreateOrder(request);
    
    // Assert
    Assert.NotNull(result);
    mockRepository.Verify(x => x.SaveAsync(), Times.Once);
}
```

---

### **TYDZIEŃ 3: Polish** (2-3 dni)

#### Dzień 13-14: API Improvements
```bash
Priority: 🟡 MEDIUM
Time: 4-5 godzin
```

**Co robić:**
1. Dodaj Pagination (Skip/Take)
2. Dodaj Filtering (produkty po kategorii, cenie)
3. Dodaj Sorting (by price, date, name)
4. Dokumentacja Swagger (descriptions, examples)

**Przykład:**
```csharp
[HttpGet]
public async Task<IActionResult> GetProducts(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? category = null,
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null,
    [FromQuery] string? sortBy = "name")
{
    var spec = new ProductFilterSpecification(category, minPrice, maxPrice, sortBy, page, pageSize);
    var products = await _repository.GetAsync(spec);
    return Ok(products);
}
```

---

#### Dzień 15: Code Review + Cleanup
```bash
Priority: 🟡 MEDIUM
Time: 3-4 godziny
```

**Co robić:**
1. Przejrzyj cały kod
2. Usunięcie dead code
3. Popraw naming conventions
4. Dodaj XML documentation
5. Sprawdź SOLID principles

---

## 📋 PRIORITY ORDER (Co robić w kolejności):

```
1️⃣  Exception Handling + Validation  (DAY 1-2)
2️⃣  Logging (Serilog)               (DAY 3-4)
3️⃣  JWT + Auth                       (DAY 5-6)
4️⃣  Authorization (Roles)            (DAY 7)
5️⃣  Service Layer                    (DAY 8-10)
6️⃣  Unit Tests                       (DAY 11-12)
7️⃣  API Improvements (Pagination)    (DAY 13-14)
8️⃣  Code Review                      (DAY 15)
```

---

## 🔍 QUALITY CHECKLIST (MID-Level Requirements)

### Code Quality
- [ ] SOLID principles respected
- [ ] DRY (Don't Repeat Yourself)
- [ ] Clear naming conventions
- [ ] No magic strings/numbers
- [ ] Proper async/await usage
- [ ] Exception handling everywhere

### API Design
- [ ] RESTful endpoints
- [ ] Consistent error responses
- [ ] Proper HTTP status codes
- [ ] API versioning ready
- [ ] Swagger documentation complete
- [ ] Rate limiting in place

### Security
- [ ] JWT with proper claims
- [ ] Password hashing (BCrypt)
- [ ] SQL injection prevention (EF Core)
- [ ] CORS properly configured
- [ ] Sensitive data not logged
- [ ] Secrets in .env (not in code)

### Testing
- [ ] Unit tests for business logic
- [ ] >70% code coverage
- [ ] Happy path + edge cases tested
- [ ] Mocks used correctly
- [ ] Integration tests for DB

### DevOps
- [ ] Docker working
- [ ] Docker Compose for local dev
- [ ] CI/CD ready (GitHub Actions placeholder)
- [ ] Logging to file
- [ ] Health checks implemented

---

## 🎬 PRZED KAŻDYM DNIEM:

```powershell
# 1. Sprawdzić Docker
docker-compose ps
docker-compose logs -f

# 2. Uruchomić aplikację
dotnet run --project API

# 3. Testować na http://localhost:9080/swagger

# 4. Commitować regularnie
git add .
git commit -m "Add feature X"
git push origin master
```

---

## ✅ GIT COMMIT MESSAGES (dobre praktyki):

```
git commit -m "feat: Add exception handling middleware"
git commit -m "feat: Implement JWT token refresh"
git commit -m "test: Add unit tests for OrderService"
git commit -m "refactor: Extract business logic to service layer"
git commit -m "docs: Update API documentation"
```

---

## 🚨 CZY ROBIĆ PUSH?

### **TAK - ale ze strategią:**

```powershell
# Zanim push:
1. Uruchom aplikację - musi działać
2. Sprawdź Docker - musi startować
3. Compile - żadnych errorów
4. Commit message - ma sens

# Push:
git push origin master

# Lepiej - gałąź feature:
git checkout -b feature/exception-handling
# ... zrób zmiany ...
git push origin feature/exception-handling
# Potem: utwórz Pull Request w GitHub
```

**REKOMENDACJA:** Rób `push` co dzień, ale na gałęzi `feature/`, a do `master` merge gdy feature gotowy.

---

## 📈 PROGRES ROADMAPU

```
Week 1:  Exception Handling + Auth        [████░░░░░░] 40%
Week 2:  Business Logic + Testing         [████████░░] 80%
Week 3:  Polish + Documentation           [██████████] 100%

Cel: MID-LEVEL BACKEND DEVELOPER
```

---

## 💡 TIPS DO SUKCESU:

1. **Jeden feature na dzień** - nie rób wszystkiego na raz
2. **Push codziennie** - mały commit > duży commit
3. **Test podczas pisania** - nie na końcu
4. **Dokumentuj się** - na każdym kroku
5. **Stack Overflow to Twój przyjaciel** - nie wstydzę się pytać
6. **Code Review siebie** - przejrzyj kod zanim commiitasz
7. **Refactor is OK** - improve existing code
8. **Git commits are messages** - przyszły Ty będzie wdzięczny

---

## 🎓 RECURSOS (zasoby):

- **Exception Handling**: https://github.com/Karol8284/E-COMMERCE/wiki/Exception-Handling
- **JWT Auth**: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn
- **Testing**: https://xunit.net/docs/getting-started/netcore
- **Logging**: https://github.com/serilog/serilog/wiki
- **SOLID**: https://en.wikipedia.org/wiki/SOLID

---

## 🎯 KONIEC TYGODNIA 3 - BĘDZIESZ MIEĆ:

✅ Production-ready API  
✅ Comprehensive tests  
✅ Professional error handling  
✅ Secure authentication  
✅ Clean code  
✅ Docker deployment ready  

**→ READY FOR MID-LEVEL CODE REVIEW**
