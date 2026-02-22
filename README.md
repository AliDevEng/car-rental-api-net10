# CarRental Backend - Mr Rent

A clean architecture .NET 10 Web API for a car rental management system.

## ✅ Iteration 1 - COMPLETED

### What was built:
- ✅ Clean architecture structure (Core, Infrastructure, Application, API)
- ✅ Entity Framework Core setup with MySQL
- ✅ Database entities: Car, CarsCategory, Customer, Admin, Rental, Payment
- ✅ DbContext with relationships and seed data (4 categories)
- ✅ Categories API endpoint
- ✅ Initial database migration
- ✅ CORS configuration for frontend
- ✅ API project configuration

### Project Structure:
```
CarRental/
├── CarRental.API/              # Web API Layer
│   ├── Controllers/
│   │   └── CategoriesController.cs
│   ├── Program.cs
│   └── appsettings.json
├── CarRental.Core/             # Domain Layer
│   └── Entities/
│       ├── Car.cs
│       ├── CarsCategory.cs
│       ├── Customer.cs
│       ├── Admin.cs
│       ├── Rental.cs
│       └── Payment.cs
├── CarRental.Infrastructure/   # Data Access Layer
│   ├── Data/
│   │   ├── CarRentalDbContext.cs
│   │   └── Migrations/
│   │       └── InitialCreate.cs
└── CarRental.Application/      # Business Logic Layer (will be used in future iterations)
```

## 🗄️ Database Setup

Before running the application, create the MySQL database:

```sql
CREATE DATABASE mr_rent CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'mr_rent'@'localhost' IDENTIFIED BY 'password123';
GRANT ALL PRIVILEGES ON mr_rent.* TO 'mr_rent'@'localhost';
FLUSH PRIVILEGES;
```

## 🚀 How to Run

1. **Update database connection** in `appsettings.json` if needed
2. **Apply migrations** to create database schema:
   ```bash
   dotnet ef database update --project CarRental.Infrastructure --startup-project CarRental.API
   ```
3. **Run the API**:
   ```bash
   dotnet run --project CarRental.API
   ```
4. **Test endpoints** using the `.http` file or browse to https://localhost:7174/

## 📡 Available Endpoints

### Categories
- `GET /categories` - Get all car categories
- `GET /categories/{id}` - Get category by ID

## 🧪 Testing

Use the `CarRental.API.http` file to test endpoints directly in Visual Studio or VS Code with REST Client extension.

## 📦 NuGet Packages Used

- Microsoft.EntityFrameworkCore (10.0.3)
- MySql.EntityFrameworkCore (10.0.1)
- Microsoft.EntityFrameworkCore.Design (10.0.3)

## 🎯 Next Steps (Iteration 2)

- [ ] Create Car entity endpoints (CRUD)
- [ ] Add filtering and sorting to Cars list
- [ ] Add car images support
- [ ] Test and commit

## 🛠️ Development Commands

```bash
# Build the solution
dotnet build

# Run tests
dotnet test

# Create a new migration
dotnet ef migrations add MigrationName --project CarRental.Infrastructure --startup-project CarRental.API

# Apply migrations
dotnet ef database update --project CarRental.Infrastructure --startup-project CarRental.API

# Remove last migration
dotnet ef migrations remove --project CarRental.Infrastructure --startup-project CarRental.API
```

## 📝 Notes

- Using **Clean Architecture** for scalability and maintainability
- **Separation of Concerns**: Each layer has a specific responsibility
- **Entity Framework Core** with code-first approach
- **Repository pattern** will be introduced in future iterations
- **DTOs** will be added when implementing business logic

---

**Built with ❤️ for learning .NET Core**
