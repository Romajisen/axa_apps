# Product API (.NET Backend)

A simple ASP.NET Core Web API for managing products with authentication and caching.

## Requirements

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Optional: [Visual Studio 2022+](https://visualstudio.microsoft.com/) or VS Code

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/romajisen/axa_apps.git
cd axa_apps/ProductApi
```

### 2. Setup the Database

- Create a local SQL Server database named `ProductDb` or update the connection string in `appsettings.json`.

Example `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ProductDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 3. Apply Migrations

```bash
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```

API will be available at: `http://localhost:5151`

## Endpoints

### Authentication

- `POST /api/auth/register` – Register a user
- `POST /api/auth/login` – Login and receive JWT token

### Products (Protected by JWT)

- `GET /api/products`
- `GET /api/products/{id}`
- `GET /api/products/search?name=abc&minPrice=10&maxPrice=100`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`

Include the token in the `Authorization` header:

```http
Authorization: Bearer YOUR_TOKEN
```

## Notes

- Caching uses in-memory cache.
- Token expiration and role are extracted from JWT claims.

## License

MIT