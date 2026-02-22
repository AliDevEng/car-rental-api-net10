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
- ✅ **Car Inventory Management** - Browse cars by category (Budget, Economy, SUV, Transport)
- ✅ **RESTful API** - Clean, predictable endpoints following REST conventions
- ✅ **Database Migrations** - Code-first approach with Entity Framework Core
- ✅ **CORS Configuration** - Ready for frontend integration
- ✅ **Seed Data** - Pre-populated car categories for testing
- ✅ **Clean Architecture** - Separation of concerns with distinct layers

### Upcoming Features
- 🔜 **JWT Authentication** - Secure user and admin authentication
- 🔜 **Car CRUD Operations** - Full management of car inventory
- 🔜 **Booking System** - Create and manage rental reservations
- 🔜 **Payment Processing** - Handle payment transactions
- 🔜 **Admin Dashboard** - Administrative endpoints for system management

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
CarRental/
├── CarRental.API/              # Presentation Layer
│   ├── Controllers/            # API endpoints
│   └── Program.cs              # Application configuration
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
├── CarRental.Infrastructure/   # Data Access Layer
│   ├── Data/
│   │   └── CarRentalDbContext.cs
│   └── Migrations/             # EF Core migrations
│
└── CarRental.Application/      # Business Logic Layer
    └── (Future: Services, DTOs, Validators)
```

**Design Patterns:**
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

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/categories` | Get all car categories |
| `GET` | `/categories/{id}` | Get category by ID |

**Example Request:**

```http
GET https://localhost:7174/categories
Accept: application/json
```

**Example Response:**

```json
[
  {
    "id": 1,
    "name": "Budget",
    "cars": []
  },
  {
    "id": 2,
    "name": "Economy",
    "cars": []
  }
]
```

### Coming Soon

- 🔜 `POST /auth/register` - User registration
- 🔜 `POST /auth/login` - User authentication
- 🔜 `GET /cars` - List all available cars
- 🔜 `POST /rentals` - Create a booking
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

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.EntityFrameworkCore | 10.0.3 | ORM framework |
| MySql.EntityFrameworkCore | 10.0.1 | MySQL provider |
| Microsoft.EntityFrameworkCore.Design | 10.0.3 | Migration tools |

---

## 🚧 Roadmap

### Phase 1: Foundation ✅ (Current)
- [x] Clean architecture setup
- [x] Database schema design
- [x] Categories API
- [x] CORS configuration
- [x] Initial migration

### Phase 2: Core Features 🔄 (In Progress)
- [ ] Cars CRUD endpoints
- [ ] Filtering and pagination
- [ ] Image upload support
- [ ] Advanced search

### Phase 3: Authentication & Authorization
- [ ] JWT implementation
- [ ] User registration/login
- [ ] Role-based access control
- [ ] Password hashing with BCrypt

### Phase 4: Booking System
- [ ] Rental creation
- [ ] Availability checking
- [ ] Booking management
- [ ] Status tracking

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
- **CarRental.Application** - Business logic, DTOs, service interfaces (future)

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
