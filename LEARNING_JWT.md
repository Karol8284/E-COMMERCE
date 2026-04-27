# 🎓 JWT Refresh Tokens - Kompletna Nauka od Zera

## 📌 Część 1: Co to jest JWT?

### Teoria - JWT (JSON Web Token)

JWT to **token** (ciąg tekstowy) który zawiera informacje o użytkowniku. Jest jak "bilet" który użytkownik dostaje zaraz po logowaniu.

```
Struktura JWT:
┌─────────────────┬──────────────────────┬──────────────────────┐
│    HEADER       │      PAYLOAD         │      SIGNATURE       │
│ (algoritm)      │ (dane użytkownika)   │ (potwierdzenie)      │
└─────────────────┴──────────────────────┴──────────────────────┘
```

**Przykład rzeczywisty:**
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

Jeśli go zdekodujemy, zobaczymy:
```json
{
  "sub": "1234567890",
  "name": "John Doe",
  "iat": 1516239022
}
```

---

## 🔑 Część 2: Dwa Tokeny - Dlaczego Dwa?

### Problem: Krótki vs Długi Token

Wyobraź sobie:
- **Access Token** - krótko żyjący (15 minut)
  - Używany do każdego żądania do API
  - Szybko wygasa = jeśli ktoś go ukradnie, ma tylko 15 minut
  - **Bezpieczny!** ✅

- **Refresh Token** - długo żyjący (7 dni)
  - Przechowywany bezpiecznie (co 7 dni się zmienia)
  - Używany TYLKO aby uzyskać nowy Access Token
  - **Też bezpieczny!** ✅

### Jak to wygląda w praktycie:

```
UŻYTKOWNIK LOGUJE SIĘ
        ↓
  /api/auth/login
        ↓
    [SERVER]
        ↓
   Tworzę 2 tokeny:
   - AccessToken (ważny 15 min)
   - RefreshToken (ważny 7 dni)
        ↓
ZWRACAM OBA TOKENY
        ↓
PRZEGLĄDAR ZAPISUJE
        ↓

DEPOIS 14 MINUT - AccessToken się kończy
        ↓
PRZEGLĄDARKA WYSYŁA:
/api/auth/refresh + RefreshToken
        ↓
[SERVER SPRAWDZA]
- Czy RefreshToken jest ważny?
- Czy użytkownik istnieje?
- Czy konto jest aktywne?
        ↓
TWORZĘ NOWY AccessToken + nowy RefreshToken
        ↓
STARY RefreshToken INVALIDUJĘ (dodaję do czarnej listy)
        ↓
ZWRACAM NOWE TOKENY
```

---

## 🛠️ Część 3: Jak Napisać to od Zera

### KROK 1: Stworzyć Interface

Interface to **kontrakt** - mówi "co ta klasa musi robić".

```csharp
// 📄 IJwtTokenService.cs
public interface IJwtTokenService
{
    // Musimy mieć te 4 metody - bo tyle rzeczy będziemy robić z tokenami
    Task<JwtTokens> GenerateTokensAsync(User user);
    Task<ClaimsPrincipal?> ValidateTokenAsync(string token);
    Task RevokeTokenAsync(string refreshToken);
    Task<bool> IsTokenRevokedAsync(string refreshToken);
}
```

**Czemu async i Task?** Ponieważ tokenizacja może trwać - chcemy "czekać" bez blokkowania.

### KROK 2: Model do Zwrócenia

Kiedy generujemy tokeny, musimy zwrócić oba w **jednym obiekcie**:

```csharp
// 📄 JwtTokens.cs (DTO)
public class JwtTokens
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public long ExpiresIn { get; set; }  // w sekundach
}
```

### KROK 3: Implementacja Serwisu - Funkcja Generowania

To **serce** całego systemu:

```csharp
// 📄 JwtTokenService.cs

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;      // Czyta z appsettings.json
    private readonly ILogger<JwtTokenService> _logger;   // Loguje operacje
    private static readonly HashSet<string> RevokedTokens = new();  // Czarna lista
    
    private const int AccessTokenExpirationMinutes = 15;
    private const int RefreshTokenExpirationDays = 7;
    
    // KONSTRUKTOR - wstrzykiwanie zależności (DI)
    public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
    {
        _configuration = configuration;  // Teraz mam dostęp do config
        _logger = logger;               // Mogę logować
    }

    /// Funkcja 1: GENEROWANIE TOKENÓW
    public async Task<JwtTokens> GenerateTokensAsync(User user)
    {
        try
        {
            // KROK 1: Wczytaj sekret z appsettings.json
            var jwtSecret = _configuration["Jwt:Secret"] 
                ?? throw new InvalidOperationException("JWT Secret not configured");
            
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "E-Commerce-API";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "E-Commerce-App";

            // KROK 2: Stwórz klucz szyfrowania
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            // 🔐 Co to znaczy?
            // - SymmetricSecurityKey = ten sam klucz do szyfrowania i odszyfrowywania
            // - HmacSha256 = algorytm szyfrowania (stanowisko przemysłowe)

            // KROK 3: Stwórz Claims (informacje o użytkowniku w tokenie)
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.DisplayName),
                new(ClaimTypes.Role, user.Role.ToString()),
                new("IsEmailConfirmed", user.IsEmailConfirmed.ToString()),
            };
            
            // 💡 Claims = dane które ZAWIERA token
            // Kiedy token będzie walidowany, będą one dostępne

            // KROK 4: ACCESS TOKEN (krótkotrwały)
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes);
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = accessTokenExpiration,
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);
            
            // 🔄 Co się tutaj dzieje?
            // 1. Tworzymy "descriptor" (przepis na token)
            // 2. CreateToken() - tworzy token object
            // 3. WriteToken() - zamienia go na string (ten ciąg z trzema częściami)

            // KROK 5: REFRESH TOKEN (długotrwały, random string)
            var refreshToken = GenerateRefreshToken();
            // Będzie to losowy ciąg 32 bajtów w Base64

            _logger.LogInformation(
                "✓ Generated tokens for user {UserId} ({Email}). Access token expires at {ExpiresAt}",
                user.Id, user.Email, accessTokenExpiration
            );

            return await Task.FromResult(new JwtTokens
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshToken,
                ExpiresIn = (long)(accessTokenExpiration - DateTime.UtcNow).TotalSeconds
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error generating tokens for user {UserId}", user.Id);
            throw;
        }
    }
}
```

**Analiza linijka po linijce:**

| Linia | Co robi | Dlaczego |
|-------|--------|---------|
| `_configuration["Jwt:Secret"]` | Czyta z pliku JSON | Sekret nie może być w kodzie! |
| `SymmetricSecurityKey` | Tworzy klucz | Potrzebny do szyfrowania tokena |
| `Claim` | Dodaje dane do tokena | Token zawiera imię, email, role |
| `AddMinutes(15)` | Token wygaśnie za 15 min | Bezpieczeństwo |
| `tokenHandler.WriteToken()` | Zamienia token w string | Żeby wysłać do klienta |

### KROK 4: Walidacja Tokena

```csharp
/// Funkcja 2: WALIDACJA (czy token jest prawdziwy?)
public async Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
{
    try
    {
        var jwtSecret = _configuration["Jwt:Secret"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var tokenHandler = new JwtSecurityTokenHandler();

        // 🔍 Sprawdzamy token
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,   // Czy klucz jest prawidłowy?
            IssuerSigningKey = key,            // Jaki klucz użyć
            ValidateIssuer = false,            // (na produkcji: true)
            ValidateAudience = false,          // (na produkcji: true)
            ValidateLifetime = true,           // Czy token nie wygasł?
            ClockSkew = TimeSpan.Zero          // Bez "przebaczenia" dla czasem
        }, out SecurityToken validatedToken);

        return await Task.FromResult(principal);
    }
    catch (Exception ex)
    {
        _logger.LogWarning(ex, "⚠️ Token validation failed");
        return null;  // Zwracamy null = token jest złych
    }
}
```

**Co się sprawdza:**
- ✅ Czy token był podpisany naszym sekretem? (nikt go nie zmienił)
- ✅ Czy token nie wygasł?
- ✅ Czy data/godzina się zgadza?

### KROK 5: Czarna Lista (Revocation)

```csharp
/// Funkcja 3: INVALUDOWANIE STAREGO TOKENA
public async Task RevokeTokenAsync(string refreshToken)
{
    RevokedTokens.Add(refreshToken);  // Dodaj do czarnej listy
    _logger.LogInformation("🔐 Refresh token revoked");
    await Task.CompletedTask;
}

/// Funkcja 4: SPRAWDZENIE CZY TOKEN NA CZARNEJ LIŚCIE
public async Task<bool> IsTokenRevokedAsync(string refreshToken)
{
    return await Task.FromResult(RevokedTokens.Contains(refreshToken));
}
```

**Wyjaśnienie:**
```
RevokedTokens = HashSet
┌────────────────────────────────────────────────────┐
│ Stare tokeny które invaludowaliśmy                 │
│                                                    │
│ - abc123xyz...                                     │
│ - def456uvw...                                     │
│ - ghi789rst...                                     │
│                                                    │
│ Kiedy ktoś chce odświeżyć token, sprawdzamy       │
│ czy jest w tej liście. Jeśli TAK → ODRZUCAMY      │
└────────────────────────────────────────────────────┘
```

**PROBLEMY:** Ta czarna lista jest w **RAM pamięci**. Jeśli serwer się restartuje - historia znika! 
**ROZWIĄZANIE na produkcji:** Zapisujemy w bazie danych ✅

---

## 🔌 Część 4: Integracja z Controlle

### Jak użyć tego serwisu w AuthController:

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;  // Wstrzykniemy to
    private readonly ILogger<AuthController> _logger;

    // KONSTRUKTOR - DI wstrzykuje zależności
    public AuthController(
        AppDbContext context, 
        IJwtTokenService jwtTokenService,    // ← Otrzymujemy serwis
        ILogger<AuthController> logger)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;  // ← Zapamiętujemy
        _logger = logger;
    }

    /// ENDPOINT 1: LOGIN - Zwracamy tokeny
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        // 1. Sprawdzamy czy użytkownik istnieje i hasło jest OK
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower());

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return Unauthorized(new { success = false, message = "Invalid credentials" });

        // 2. Generujemy tokeny
        var tokens = await _jwtTokenService.GenerateTokensAsync(user);
        
        // 3. Zwracamy
        return Ok(new
        {
            success = true,
            accessToken = tokens.AccessToken,
            refreshToken = tokens.RefreshToken,
            expiresIn = 900,  // 15 minut
            user = new { user.Id, user.Email, user.DisplayName }
        });
    }

    /// ENDPOINT 2: REFRESH - Uzyskaj nowy AccessToken
    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        // 1. Sprawdź czy token nie jest na czarnej liście
        var isRevoked = await _jwtTokenService.IsTokenRevokedAsync(request.RefreshToken);
        if (isRevoked)
            return Unauthorized(new { success = false, message = "Token revoked" });

        // 2. Waliduj token
        var principal = await _jwtTokenService.ValidateTokenAsync(request.RefreshToken);
        if (principal == null)
            return Unauthorized(new { success = false, message = "Invalid token" });

        // 3. Wyciągnij ID użytkownika z tokena
        var userIdClaim = principal.FindFirst("nameid")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { success = false, message = "Invalid token claims" });

        // 4. Sprawdź że użytkownik istnieje
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null || !user.IsActive)
            return Unauthorized(new { success = false, message = "User not found or inactive" });

        // 5. Wygeneruj NOWE tokeny
        var newTokens = await _jwtTokenService.GenerateTokensAsync(user);
        
        // 6. INVALUDUJ STARY refreshToken
        await _jwtTokenService.RevokeTokenAsync(request.RefreshToken);

        // 7. Zwróć nowe
        return Ok(new
        {
            success = true,
            accessToken = newTokens.AccessToken,
            refreshToken = newTokens.RefreshToken,
            expiresIn = 900
        });
    }
}
```

---

## 💉 Część 5: Dependency Injection (DI) - Rejestracja

To najważniejsze! Jeśli nie zarejestrujesz - serwis nie będzie dostępny.

```csharp
// 📄 Program.cs

var builder = WebApplicationBuilder.CreateBuilder(args);

// 1. Inne rzeczy...
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. ← TUTAJ REJESTRUJEMY JWT SERVICE! ← 
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// 3. Zbuduj app
var app = builder.Build();
```

**Wyjaśnienie:**
```
AddScoped<IJwtTokenService, JwtTokenService>()

"Hej ASP.NET, kiedy coś poprosi o IJwtTokenService,
  daj mu nową instancję JwtTokenService"

┌─────────────────────────────────────────┐
│ AuthController potrzebuje IJwtTokenService
│            ↓
│ ASP.NET sprawdza: "Mam registrację?"
│            ↓
│ TAK! Tworzy: new JwtTokenService(config, logger)
│            ↓
│ Daje to do AuthController konstruktora
└─────────────────────────────────────────┘
```

---

## ⚙️ Część 6: Konfiguracja

```json
// 📄 appsettings.json

{
  "Jwt": {
    "Secret": "your-super-secret-key-change-in-production-minimum-32-characters",
    "Issuer": "E-Commerce-API",
    "Audience": "E-Commerce-App"
  }
}
```

**WAŻNE:**
- `Secret` - Twój prywatny klucz! NIGDY go nie commituj do publicznego repo!
- `Issuer` - Kto wydał token (twój serwis)
- `Audience` - Kto ma go używać (twoja aplikacja)

---

## 🔄 Część 7: Cały Flow Krok Po Kroku

```
USER ACTIONS          SERVER                RESPONSE
─────────────────────────────────────────────────────

1. LOGOWANIE
Wpisze email/hasło        Login request
                          ↓
                    Sprawdź hasło BCrypt
                    Wygeneruj 2 tokeny
                          ↓
              ← AccessToken (15 min)
              ← RefreshToken (7 dni)

2. API REQUESTS (następne 15 minut)
Dodaj header:
Authorization: Bearer AccessToken
                    Sprawdzam token
                          ↓
                    Udzielam dostępu
                    ← 200 OK + dane

3. ACCESSTOKEN WYGASA (15 minut później)
GET /products         
Authorization: Bearer AccessToken
(STARY, wygasły)     ← 401 UNAUTHORIZED

4. ODŚWIEŻENIE
POST /refresh
RefreshToken          Sprawdzam:
                      1. Czy na czarnej liście?
                      2. Czy walidny?
                      3. Czy użytkownik OK?
                      4. Wygeneruj NOWE tokeny
                      5. Invaluduj stary
                          ↓
              ← Nowy AccessToken
              ← Nowy RefreshToken

5. DALEJ API (następne 15 minut)
GET /products         
Authorization: Bearer AccessToken
(NOWY, świeży)       ← 200 OK + dane
```

---

## 🎯 Ćwiczenie dla Ciebie

Spróbuj napisać te funkcje od zera:

```csharp
// ❓ ZADANIE 1: Napisz funcję GenerateRefreshToken
// - Powinna zwrócić random string (32 bajty w Base64)
// - Hint: RandomNumberGenerator.Create()

// ❓ ZADANIE 2: Napisz endpoint POST /logout
// - Powinna invaludować BIEŻĄCY accessToken
// - (Hint: wymaga zmian w systemie revocation)

// ❓ ZADANIE 3: Zmodyfikuj ValidateTokenAsync
// - Aby zwracała konkretne błędy (EXPIRED vs INVALID)
```

---

## 📚 Słownik

| Termin | Wyjaśnienie |
|--------|------------|
| **JWT** | Token zawierający dane podpisane kryptograficznie |
| **Claims** | Dane zawarte w tokenie (email, role, itd) |
| **Signature** | Potwierdzenie że nikt nie zmienił tokena |
| **AccessToken** | Krótkotrwały token do API requests |
| **RefreshToken** | Długotrwały token do uzyskania nowego AccessToken |
| **DI (Dependency Injection)** | ASP.NET daje ci zależności zamiast je tworzyć |
| **Scoped** | Jedna instancja na jedno HTTP request |
| **Async/Await** | Czekamy na wynik bez blokkowania |
| **SymmetricSecurityKey** | Ten sam klucz do szyfrowania i odszyfrowywania |
| **BCrypt** | Bezpieczne haszowanie haseł |

---

## 🚀 Następny Krok

Kiedy będziesz gotowy na Day 3:
- Dodaj `[Authorize]` atrybuty do endpoints
- Sprawdzaj `User.Identity?.Name` w kontrollerach
- Dodaj role-based checks: `[Authorize(Roles = "Admin")]`

---

## 💡 Pytania?

Jeśli coś nie jasne:
1. Czytaj kod z **komentarzami** w JwtTokenService.cs
2. Uruchom w Swagger UI i zobacz request/response
3. Dodaj breakpoint i step through w debuggerze
4. Czytaj Serilog logs aby zobaczyć co się dzieje

**PAMIĘTAJ:** Najlepszy sposób to PRAKTYKA! Spróbuj napisać prosty token service od zera! 💪
