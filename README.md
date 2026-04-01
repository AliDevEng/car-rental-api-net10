# Car Rental Management System - Backend API

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-14.0-239120?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql&logoColor=white)](https://www.mysql.com/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-10.0-512BD4)](https://docs.microsoft.com/en-us/ef/core/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A modern, scalable **RESTful API** for car rental management built with **.NET 10** and **Clean Architecture** principles. This backend provides comprehensive endpoints for managing car inventory, customer bookings, payments, and administrative operations.

> **Full-Stack Project:** This is the backend API. Frontend repository: [Coming Soon]

---

## 🚀 Features

### Current Features
- ✅ **Car Inventory Management** - Full CRUD for cars with category browsing
- ✅ **Filtering & Pagination** - Filter by brand, fuel, transmission, price range; sorted & paged results
- ✅ **Customer Management** - Register customers and manage profiles
- ✅ **Admin Dashboard** - Dashboard statistics and customer listing endpoints
- ✅ **Input Validation** - FluentValidation with detailed field-level error messages
- ✅ **Global Error Handling** - Consistent JSON error responses (400, 401, 404, 409, 500)
- ✅ **RESTful API** - Clean, predictable endpoints following REST conventions
- ✅ **Database Migrations** - Code-first approach with Entity Framework Core
- ✅ **CORS Configuration** - Ready for frontend integration
- ✅ **Seed Data** - Pre-populated car categories for testing
- ✅ **Clean Architecture** - Separation of concerns with distinct layers
- ✅ **JWT Authentication** - Secure token-based authentication for customers and admins
- ✅ **Password Hashing** - BCrypt password hashing for secure credential storage
- ✅ **Role-Based Access Control** - Admin-only and authenticated-user endpoint protection
- ✅ **Booking System** - Rental creation, availability checks, booking management, and status tracking

### Upcoming Features
- 🔜 **Payment Processing** - Handle payment transactions

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
CarRental/
├── CarRental.API/              # Presentation Layer
│   ├── Controllers/            # API endpoints
│   │   ├── AuthController.cs
│   │   ├── CategoriesController.cs
│   │   ├── CarsController.cs
│   │   ├── CustomersController.cs
│   │   ├── RentalsController.cs
│   │   └── AdminController.cs
│   ├── Middleware/             # Global exception handling
│   │   └── GlobalExceptionMiddleware.cs
│   └── Program.cs             # Application configuration
│
├── CarRental.Application/      # Business Logic Layer
│   ├── DTOs/                   # Data Transfer Objects
│   │   ├── Auth/               # Auth request/response DTOs
│   │   ├── Car/                # Car request/response DTOs
│   │   ├── Customer/           # Customer request/response DTOs
│   │   ├── Rental/             # Rental request/response DTOs
│   │   ├── Admin/              # Admin response DTOs
│   │   └── Common/             # Shared DTOs (PagedResult)
│   ├── Exceptions/             # Custom exceptions
│   ├── Services/               # Business logic services
│   │   └── Interfaces/         # Service contracts
│   └── Validators/             # FluentValidation validators
│
├── CarRental.Core/             # Domain Layer
│   └── Entities/               # Business entities
│       ├── Car.cs
│       ├── CarsCategory.cs
│       ├── Customer.cs
│       ├── Admin.cs
│       ├── Rental.cs
│       └── Payment.cs
│
└── CarRental.Infrastructure/   # Data Access Layer
    ├── Data/
    │   └── CarRentalDbContext.cs
    └── Migrations/             # EF Core migrations
```

**Design Patterns:**
- Service Layer Pattern
- Repository Pattern (planned)
- Dependency Injection
- Code-First Database Design

---

## 🛠️ Tech Stack

| Category | Technology |
|----------|-----------|
| **Framework** | .NET 10 |
| **Language** | C# 14.0 |
| **Database** | MySQL 8.0 |
| **ORM** | Entity Framework Core 10.0 |
| **API Style** | RESTful |
| **Architecture** | Clean Architecture |

---

## 📋 Prerequisites

Before running this project, ensure you have:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MySQL 8.0+](https://dev.mysql.com/downloads/) or Docker with MySQL
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

---

## ⚙️ Installation & Setup

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/AliDevEng/car-rental-api-net10.git
cd car-rental-api-net10
```

### 2️⃣ Configure Database

**Option A: Using Docker (Recommended)**

```bash
docker run --name mysql-carrental -e MYSQL_ROOT_PASSWORD=password -p 3307:3306 -d mysql:8.0
```

**Option B: Local MySQL Installation**

Create the database manually:

```sql
CREATE DATABASE mr_rent CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'your_user'@'localhost' IDENTIFIED BY 'your_password';
GRANT ALL PRIVILEGES ON mr_rent.* TO 'your_user'@'localhost';
FLUSH PRIVILEGES;
```

### 3️⃣ Update Connection String

Edit `CarRental.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3307;Database=mr_rent;User=root;Password=password;"
  }
}
```

### 4️⃣ Apply Database Migrations

```bash
dotnet ef database update --project CarRental.Infrastructure --startup-project CarRental.API
```

### 5️⃣ Run the Application

```bash
dotnet run --project CarRental.API
```

The API will be available at: **https://localhost:7174**

---

## 📡 API Endpoints

### Categories

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/categories` | Get all car categories | 🌐 Public |
| `GET` | `/categories/{id}` | Get category by ID (includes cars) | 🌐 Public |

### Cars

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/cars` | Get all cars (filtered, sorted, paged) | 🌐 Public |
| `GET` | `/cars/{id}` | Get car by ID | 🌐 Public |
| `POST` | `/cars` | Create a new car | 🔒 Admin |
| `PUT` | `/cars/{id}` | Update an existing car | 🔒 Admin |
| `DELETE` | `/cars/{id}` | Delete a car | 🔒 Admin |

**Query Parameters for `GET /cars`:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `brand` | string | Filter by brand (partial match) |
| `categoryId` | int | Filter by category |
| `fuel` | string | Filter by fuel type (Petrol, Diesel, Electric, Hybrid) |
| `transmission` | string | Filter by transmission (Manual, Automatic) |
| `minPrice` | decimal | Minimum price filter |
| `maxPrice` | decimal | Maximum price filter |
| `status` | string | Filter by status (Available, Rented, Maintenance) |
| `page` | int | Page number (default: 1) |
| `pageSize` | int | Items per page (default: 10) |
| `sortBy` | string | Sort field: id, brand, model, price, year (default: id) |
| `sortDirection` | string | asc or desc (default: asc) |

### Authentication

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/auth/register` | Register a new customer account | 🌐 Public |
| `POST` | `/auth/login` | Customer login | 🌐 Public |
| `POST` | `/auth/admin/login` | Admin login | 🌐 Public |

### Customers

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/customers` | Create a customer (admin) | 🔒 Admin |
| `GET` | `/customers/{id}` | Get customer profile | 🔒 Authenticated |
| `PUT` | `/customers/{id}` | Update customer profile | 🔒 Authenticated |

### Rentals

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/rentals` | Create new booking | 🔒 Customer |
| `GET` | `/rentals/{id}` | Get booking details | 🔒 Customer/Admin |
| `GET` | `/rentals/my-rentals` | Customer rental history (paged) | 🔒 Customer |
| `GET` | `/rentals` | Get all rentals (paged) | 🔒 Admin |
| `PUT` | `/rentals/{id}/status` | Update rental status | 🔒 Admin |
| `DELETE` | `/rentals/{id}` | Cancel rental | 🔒 Customer |
| `GET` | `/rentals/availability` | Check availability for date range | 🌐 Public |

### Admin

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/admin/stats` | Get dashboard statistics | 🔒 Admin |
| `GET` | `/admin/customers` | Get all customers | 🔒 Admin |

**Example — Register:**

```http
POST https://localhost:7174/auth/register
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "securePassword123",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Example — Login Response (200):**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresAt": "2026-03-08T15:30:00Z",
  "role": "Customer",
  "userId": 1,
  "email": "john@example.com"
}
```

**Example — Create Car (requires Admin JWT):**

```http
POST https://localhost:7174/cars
Content-Type: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...

{
  "categoryId": 2,
  "brand": "Toyota",
  "model": "Corolla",
  "year": 2024,
  "regNr": "ABC-123",
  "fuel": "Petrol",
  "transmission": "Automatic",
  "seats": 5,
  "price": 250.00,
  "status": "Available"
}
```

**Example — Validation Error Response (400):**

```json
{
  "status": 400,
  "message": "One or more validation errors occurred.",
  "errors": {
    "Brand": ["Brand is required."],
    "Fuel": ["Fuel must be one of: Petrol, Diesel, Electric, Hybrid."]
  }
}
```

**Example — Unauthorized Response (401):**

```json
{
  "status": 401,
  "message": "Invalid email or password."
}
```

### Coming Soon

- 🔜 `POST /payments` - Process payment

---

## 🗄️ Database Schema

The database consists of 6 main tables with proper relationships:

- **CarsCategories** - Car category definitions (Budget, Economy, SUV, Transport)
- **Cars** - Vehicle inventory with details and pricing
- **Customers** - User accounts and profiles
- **Admins** - Administrative user accounts
- **Rentals** - Booking records and rental history
- **Payments** - Payment transactions

**Entity Relationships:**
- `Cars` → `CarsCategories` (Many-to-One)
- `Rentals` → `Cars` (Many-to-One)
- `Rentals` → `Customers` (Many-to-One)
- `Payments` → `Rentals` (One-to-One)

For detailed schema documentation, see [DATABASE_VERIFICATION.sql](DATABASE_VERIFICATION.sql)

---

## 🧪 Testing

Use the included `.http` file for endpoint testing:

1. Open `CarRental.API/CarRental.API.http` in Visual Studio or VS Code
2. Install REST Client extension (for VS Code)
3. Click "Send Request" to test endpoints

**Alternative: Using curl**

```bash
curl https://localhost:7174/categories -k
```

---

## 📦 NuGet Packages

| Package | Version | Project | Purpose |
|---------|---------|---------|---------|
| Microsoft.EntityFrameworkCore | 10.0.3 | Infrastructure | ORM framework |
| MySql.EntityFrameworkCore | 10.0.1 | Infrastructure, API | MySQL provider |
| Microsoft.EntityFrameworkCore.Design | 10.0.3 | Infrastructure, API | Migration tools |
| Microsoft.AspNetCore.OpenApi | 10.0.3 | API | OpenAPI support |
| Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.3 | API | JWT authentication middleware |
| FluentValidation | 11.11.0 | Application | Input validation |
| FluentValidation.DependencyInjectionExtensions | 11.11.0 | Application | Validator DI registration |
| BCrypt.Net-Next | 4.1.0 | Application | Password hashing |
| System.IdentityModel.Tokens.Jwt | 8.16.0 | Application | JWT token generation |
| Microsoft.Extensions.Configuration.Abstractions | 10.0.3 | Application | Configuration access |

---

## 🚧 Roadmap

### Phase 1: Foundation ✅
- [x] Clean architecture setup
- [x] Database schema design
- [x] Categories API
- [x] CORS configuration
- [x] Initial migration

### Phase 2: Core API ✅
- [x] Cars CRUD endpoints (Create, Read, Update, Delete)
- [x] Filtering, sorting, and pagination
- [x] Customer management endpoints
- [x] Admin dashboard statistics endpoint
- [x] Input validation with FluentValidation
- [x] Global error handling middleware
- [x] Proper HTTP status codes (201, 400, 404, 409, 500)

### Phase 3: Authentication & Authorization ✅
- [x] JWT implementation (HMAC-SHA256, configurable expiry)
- [x] User registration/login endpoints
- [x] Admin login endpoint
- [x] Role-based access control (Admin, Customer)
- [x] Password hashing with BCrypt
- [x] Global 401 error handling

### Phase 4: Booking System ✅
- [x] Rental creation
- [x] Availability checking
- [x] Booking management
- [x] Status tracking

### Phase 5: Payments & Deployment
- [ ] Payment gateway integration
- [ ] Transaction management
- [ ] API documentation (Swagger)
- [ ] Docker containerization
- [ ] CI/CD pipeline

---

## 🔧 Development Commands

```bash
# Build the solution
dotnet build

# Run the API
dotnet run --project CarRental.API

# Create a new migration
dotnet ef migrations add MigrationName --project CarRental.Infrastructure --startup-project CarRental.API

# Apply migrations
dotnet ef database update --project CarRental.Infrastructure --startup-project CarRental.API

# Remove last migration
dotnet ef migrations remove --project CarRental.Infrastructure --startup-project CarRental.API
```

---

## 📝 Project Structure Explained

- **CarRental.API** - Entry point, controllers, middleware configuration
- **CarRental.Core** - Domain models, business rules (framework-agnostic)
- **CarRental.Infrastructure** - Database context, migrations, repositories
- **CarRental.Application** - Business logic services, DTOs, validators, custom exceptions

This structure ensures:
- ✅ Testability
- ✅ Maintainability
- ✅ Scalability
- ✅ Clear separation of concerns

---

## 🤝 Contributing

Contributions are welcome! This is a portfolio/learning project, but feel free to:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👨‍💻 Author

**Ali Rezai**

- GitHub: [@AliDevEng](https://github.com/AliDevEng)
- LinkedIn: [https://www.linkedin.com/in/ali-rezai-314172168/]
- Portfolio: [https://github.com/AliDevEng]

---

## 🙏 Acknowledgments

- Built with **.NET 10** for learning and portfolio purposes
- Inspired by modern clean architecture practices
- Part of a full-stack car rental management system

---

**⭐ If you find this project useful, please consider giving it a star!**
