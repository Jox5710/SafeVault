# SafeVault

SafeVault is a minimal secure Web API demonstrating:
- JWT authentication and role-based authorization (RBAC)
- Secure password hashing (PBKDF2)
- Parameterized queries (Dapper) to prevent SQL injection
- Example controllers and services
- Tests and CI workflow included

## Quick start (LocalDB)
1. Requirements
   - .NET 7 SDK
   - LocalDB (or change connection string to SQLite/Postgres)

2. Initialize DB (LocalDB)
   - Open `init_db.sql` and run it in SQL Server (LocalDB) or use a migration tool.
   - Or run the provided script to create the table.

3. Run the app
```bash
dotnet restore
dotnet build
dotnet run --project SafeVault
```
The API will be available at `https://localhost:5001` (or the port shown). Open `/swagger` to test endpoints.

## Register & Login
- `POST /api/auth/register` to create a user (Admin or User role)
- `POST /api/auth/login` to get a JWT token

## Test sensitive endpoint
- `GET /api/secure/sensitive` requires role `Admin`

## Running tests
Tests are in `SafeVault.Tests` (placeholder). Run:
```bash
dotnet test
```

## CI / GitHub Actions
The project includes a GitHub Actions workflow `.github/workflows/ci.yml` that builds and runs tests on push/pull requests.

## Security notes
- Replace `JwtSettings:Key` with a strong environment-based secret in production.
- Use a managed database and secure connection strings.
- Consider adding logging, rate-limiting, and input validation libraries (e.g., FluentValidation).
