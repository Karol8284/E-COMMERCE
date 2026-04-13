# ✅ WEEKLY PROGRESS TRACKER

## Tydzień 1: Foundation (Fundamenty)

### Dzień 1-2: Exception Handling + Validation
- [ ] Utwórz `API/Middleware/ExceptionHandlingMiddleware.cs`
- [ ] Ustandaryzuj response error format
- [ ] Zainstaluj: `dotnet add package FluentValidation`
- [ ] Dodaj validatory do DTOs
- [ ] Zarejestruj w Program.cs
- [ ] ✅ COMMIT: `feat: Add global exception handling middleware`
- [ ] ✅ PUSH: do branch `feature/exception-handling`
- [ ] ✅ TEST: Prześlij invalid data - powinien być error 400

### Dzień 3-4: Logging (Serilog)
- [ ] Zainstaluj Serilog packages
- [ ] Skonfiguruj logging w Program.cs
- [ ] Dodaj structured logging w middleware
- [ ] Utwórz `logs/` folder dla log files
- [ ] ✅ COMMIT: `feat: Add Serilog structured logging`
- [ ] ✅ PUSH: do `feature/logging`
- [ ] ✅ TEST: Sprawdź `logs/` folder - powinny być pliki z logami

---

## Tydzień 2: Security & Architecture

### Dzień 5-6: JWT + Proper Auth
- [ ] Utwórz `Infrastructure/Services/JwtTokenService.cs`
- [ ] Implementuj access token + refresh token
- [ ] Dodaj Token Revocation (BlackList)
- [ ] Update AuthController z `/refresh` endpoint
- [ ] Popraw password hashing (BCrypt properly)
- [ ] ✅ COMMIT: `feat: Implement proper JWT with refresh tokens`
- [ ] ✅ PUSH: do `feature/jwt-auth`
- [ ] ✅ TEST: `curl -X POST http://localhost:9080/api/auth/refresh`

### Dzień 7: Authorization (Role-based)
- [ ] Dodaj `[Authorize(Roles = "Admin")]` do sensitive endpoints
- [ ] Implementuj Policy-based Authorization
- [ ] Utwórz Custom Authorization Attribute
- [ ] ✅ COMMIT: `feat: Add role-based authorization`
- [ ] ✅ PUSH: do `feature/rbac`
- [ ] ✅ TEST: Spróbuj dostęp bez Admin role - powinien być 403

### Dzień 8-10: Service Layer
- [ ] Utwórz `Infrastructure/Services/` folder
- [ ] Utwórz interfaces: `IOrderService`, `ICartService`, itd.
- [ ] Implementuj service classes
- [ ] Move business logic z Controllerów do Services
- [ ] Register w DI Container (Program.cs)
- [ ] ✅ COMMIT: `refactor: Extract business logic to service layer`
- [ ] ✅ PUSH: do `feature/service-layer`
- [ ] ✅ TEST: Controllers powinny być lean (10-15 linii)

### Dzień 11-12: Unit Tests
- [ ] Zainstaluj: xUnit, Moq, FluentAssertions
- [ ] Utwórz `Test/Services/` folder
- [ ] Napisz testy dla OrderService (min. 5 testów)
- [ ] Napisz testy dla CartService (min. 5 testów)
- [ ] Mock DbContext i Dependencies
- [ ] Sprawdzić coverage (aim for 70%+)
- [ ] ✅ COMMIT: `test: Add unit tests for services`
- [ ] ✅ PUSH: do `feature/unit-tests`
- [ ] ✅ TEST: `dotnet test Test.csproj`

---

## Tydzień 3: Polish & Documentation

### Dzień 13-14: API Improvements
- [ ] Dodaj Pagination (PaginationFilter, PaginationResult)
- [ ] Dodaj Filtering (ProductFilterSpecification)
- [ ] Dodaj Sorting (SortOrder, SortBy)
- [ ] Update Swagger descriptions
- [ ] Dodaj examples do DTOs
- [ ] ✅ COMMIT: `feat: Add pagination, filtering, sorting`
- [ ] ✅ PUSH: do `feature/api-improvements`
- [ ] ✅ TEST: `curl "http://localhost:9080/api/products?page=1&pageSize=10&category=Electronics&sortBy=price"`

### Dzień 15: Code Review + Cleanup
- [ ] Przejrzyj nazewnictwo - consistent?
- [ ] SOLID principles - respektowane?
- [ ] Nullable handling - wszystko covered?
- [ ] Async/await - properly used?
- [ ] XML documentation - added?
- [ ] Remove dead code
- [ ] ✅ COMMIT: `refactor: Code cleanup and improvements`
- [ ] ✅ PUSH: do `master`

---

## 📊 COMMIT CHECKLIST (każdy dzień)

Przed push - sprawdzić:

```powershell
# 1. Build succeeds
dotnet build

# 2. Docker works
docker-compose up -d
docker-compose ps

# 3. No warnings/errors
dotnet build /warnaserror

# 4. Good commit message
git log -1 --oneline

# 5. Push
git push origin feature/xxx
```

---

## 🎯 SUCCESS CRITERIA (MID-Level)

### Code Quality
- [ ] All public methods have XML documentation
- [ ] No SonarQube critical issues
- [ ] Naming conventions consistent
- [ ] No magic numbers/strings
- [ ] SOLID principles respected

### Testing
- [ ] 70%+ code coverage on business logic
- [ ] All edge cases tested
- [ ] Mocks used correctly
- [ ] Green tests on CI/CD (when ready)

### Security
- [ ] No hardcoded secrets
- [ ] Passwords hashed with BCrypt
- [ ] JWT implementation proper
- [ ] SQL injection prevented (EF Core)
- [ ] CORS configured securely

### DevOps
- [ ] Docker-Compose works
- [ ] Logs to file + console
- [ ] .env used for secrets
- [ ] Health checks implemented
- [ ] Swagger documented

### API Design
- [ ] RESTful endpoints
- [ ] Proper HTTP status codes (200, 201, 400, 401, 403, 404, 500)
- [ ] Consistent error responses
- [ ] Pagination on list endpoints
- [ ] Filtering/Sorting on complex endpoints

---

## 📈 WEEKLY SUMMARY

**End of Week 1:**
- [ ] Exception handling + Logging in place
- [ ] Basic Auth working
- [ ] Can push to GitHub confidently

**End of Week 2:**
- [ ] Service layer refactored
- [ ] Unit tests passing
- [ ] Authorization working
- [ ] Code is professional quality

**End of Week 3:**
- [ ] API fully polished
- [ ] Swagger complete
- [ ] Ready for production
- [ ] **MID-LEVEL READY** ✅

---

## 🚀 CO ROBIĆ TERAZ (First Step):

1. **Czytaj**: `ROADMAP_MID_LEVEL.md` (już masz)
2. **Zaplanuj**: Dzisiaj tylko Exception Handling + Validation
3. **Zainstaluj**: FluentValidation
4. **Commituj**: Malorzyść, ale regularnie
5. **Pytaj**: Stack Overflow jeśli się zablokujesz

---

## 📞 SOS - Co zrobić gdy się zablokujesz:

| Problem | Rozwiązanie |
|---------|------------|
| Build fails | `dotnet clean && dotnet build` |
| Docker won't start | `docker-compose down -v && docker-compose up -d` |
| Can't remember git | `git status`, `git log -1` |
| Compilation errors | Check error message - Google it! |
| Test fails | Debug with `Debug.WriteLine()` |
| Can't find class | Right-click → "Go to Definition" (F12) |

---

## 🎓 NAUKA - Co czytać:

**Dzisiaj (30 min):**
- Exception Handling best practices
- Validation in ASP.NET Core

**Jutro (30 min):**
- Serilog documentation
- Structured logging

**Seria artykułów:**
- SOLID principles
- Design patterns
- Clean code

---

**Last Update**: Today  
**Status**: 🟢 Ready to Start  
**Confidence Level**: 💪 HIGH

GO GET IT! 🚀
