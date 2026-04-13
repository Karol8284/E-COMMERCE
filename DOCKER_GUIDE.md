# Docker Compose - Professional Guide

## 📋 How It Works

### 1. **Services** (`postgres`, `api`)
- Each service = one container
- Services can communicate using their names as hostnames
- Example: `postgres` service is accessible as `postgres:5432` from `api` service

### 2. **Environment Variables** (`.env` file)
```yaml
environment:
  POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
  # Reads from .env file
  # Falls back to default: ${VAR:-default_value}
```

**Why?**
- Don't hardcode sensitive data
- Easy to switch between development/production
- CI/CD pipeline can override values

### 3. **Health Checks**
```yaml
healthcheck:
  test: ["CMD-SHELL", "pg_isready -U postgres"]
  interval: 10s
  timeout: 5s
  retries: 5
```

**Why?**
- Ensures dependencies are ready before starting dependent services
- API waits for PostgreSQL to be healthy via `depends_on: service_healthy`

### 4. **Named Volumes**
```yaml
volumes:
  postgres_data:
    driver: local
```

**Why?**
- Data persists between container restarts
- Without volumes, data is lost when container stops
- Can backup/restore easily

### 5. **Custom Networks**
```yaml
networks:
  ecommerce-network:
    driver: bridge
```

**Why?**
- Isolates your services from other Docker containers
- Services can communicate by name (DNS)
- Better security and organization

### 6. **Resource Limits**
```yaml
deploy:
  resources:
    limits:
      cpus: '2'
      memory: 1G
    reservations:
      cpus: '1'
      memory: 512M
```

**Why?**
- Prevents one service from consuming all system resources
- Ensures predictable performance
- Production requirement

---

## 🚀 Quick Start Commands

### Start services
```bash
docker-compose up -d
```

### View logs
```bash
docker-compose logs -f api        # Follow API logs
docker-compose logs -f postgres   # Follow database logs
```

### Stop services
```bash
docker-compose down              # Stop and remove containers
docker-compose down -v           # Also remove volumes (deletes data!)
```

### Rebuild image
```bash
docker-compose up -d --build
```

### Execute command in container
```bash
docker-compose exec postgres psql -U postgres -d ecommerce
docker-compose exec api dotnet migrations list
```

---

## 🔐 Security Best Practices

### Development (Local)
```bash
# Use .env file (add to .gitignore)
# Variables stored locally, not in git
```

### Production (Cloud)
```bash
# Use secrets manager:
# 1. AWS Secrets Manager
# 2. Azure Key Vault
# 3. HashiCorp Vault
# 4. Docker Secrets (Swarm mode)

# Pass via CI/CD environment
```

### DO NOT:
- ❌ Hardcode credentials in docker-compose.yml
- ❌ Commit .env to Git
- ❌ Use weak passwords
- ❌ Skip health checks
- ❌ Map unnecessary ports

### DO:
- ✅ Use .env files (development)
- ✅ Use secret managers (production)
- ✅ Implement health checks
- ✅ Set resource limits
- ✅ Use alpine images (smaller)
- ✅ Regularly update images

---

## 📊 Network Diagram

```
┌─────────────────────────────────────┐
│     Host Machine (Your Computer)    │
├─────────────────────────────────────┤
│                                     │
│  ┌────────────────────────────────┐ │
│  │  ecommerce-network (bridge)    │ │
│  │                                │ │
│  │  ┌──────────────┐              │ │
│  │  │   postgres   │              │ │
│  │  │  172.20.0.2  │              │ │
│  │  │  :5432       │              │ │
│  │  └──────────────┘              │ │
│  │         ↑                       │ │
│  │      hostname resolution        │ │
│  │    ("postgres:5432")            │ │
│  │         ↓                       │ │
│  │  ┌──────────────┐              │ │
│  │  │     api      │              │ │
│  │  │  172.20.0.3  │              │ │
│  │  │  :8080       │              │ │
│  │  └──────────────┘              │ │
│  │                                │ │
│  └────────────────────────────────┘ │
│           ↓           ↓             │
│      localhost:8080   localhost:5432 │
│      (from host)      (from host)    │
│                                     │
└─────────────────────────────────────┘
```

---

## 🧪 Testing Your Setup

### 1. Check services are running
```bash
docker-compose ps
```

Output should show:
```
NAME                COMMAND              SERVICE    STATUS
ecommerce-postgres  "docker-entrypoint…" postgres   Up (healthy)
ecommerce-api       "dotnet API.dll"     api        Up (healthy)
```

### 2. Check database connection
```bash
docker-compose exec postgres psql -U postgres -d ecommerce -c "\dt"
```

### 3. Check API is responding
```bash
curl http://localhost:8080/api/health
```

### 4. Check logs for errors
```bash
docker-compose logs --tail=100
```

---

## 🔄 Docker Compose Lifecycle

```
docker-compose up -d
    ↓
1. Read docker-compose.yml
2. Load .env variables
3. Build images (if needed)
4. Create network
5. Create volumes
6. Start postgres container
7. Run health checks on postgres
8. Start api container
9. Run health checks on api
10. Services ready!

docker-compose down
    ↓
1. Stop containers
2. Remove containers
3. Remove network
4. Keep volumes (data persists)

docker-compose down -v
    ↓
+ Remove volumes (deletes database!)
```

---

## 📝 Environment Variables Reference

| Variable | Purpose | Example |
|----------|---------|---------|
| `POSTGRES_USER` | Database user | `postgres` |
| `POSTGRES_PASSWORD` | Database password | `Mk127398` |
| `POSTGRES_DB` | Database name | `ecommerce` |
| `DB_PORT` | Exposed database port | `5432` |
| `API_HTTP_PORT` | HTTP port | `8080` |
| `API_HTTPS_PORT` | HTTPS port | `8081` |
| `ASPNETCORE_ENVIRONMENT` | App environment | `Development` |

---

## ⚠️ Common Issues

### Port already in use
```bash
# Find what's using the port
lsof -i :8080
# Kill it or change port in .env
```

### Database connection refused
- Check `DefaultConnection` string in Program.cs
- Use `Host=postgres` (not `localhost`)
- Check health checks passed

### Volume permission denied
```bash
# Run with proper permissions
sudo chown -R 999:999 ./data/postgres
```

### Container exits immediately
```bash
# Check logs
docker-compose logs api
```

---

## ✅ Production Checklist

- [ ] Remove `restart: unless-stopped` or set to `always`
- [ ] Use alpine images for all services
- [ ] Implement all health checks
- [ ] Set resource limits
- [ ] Use secret manager (not .env)
- [ ] Enable logging and monitoring
- [ ] Regular backups of volumes
- [ ] Use specific image versions (not `latest`)
- [ ] Implement SSL/TLS for databases
- [ ] Use strong passwords
- [ ] Separate .env files per environment
