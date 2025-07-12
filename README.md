# 📚 Library Management System

An ASP.NET Core Web API for managing a collection of books with secure, JWT-protected endpoints. This system allows registered users to perform CRUD operations on books.

---

## 🚀 Features

- User registration and login
- EF Core with MSSQL persistence
- Automatic database seeding with sample books
- OpenAPI/Swagger UI for API testing
- Optional search and pagination

---

## 🛠️ Tech Stack

- ASP.NET Core 7
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger (Swashbuckle)

---

## 📦 Requirements

- [.NET 7 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or full instance)

---

## 🔧 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/oluwafemioluwatuyi/LibraryManagement_Backend.git
cd LibraryManagement

```

#### 2. Configure appsettings.json

```bash
{
    "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "SQLServerDatabase": "Server=localhost,1433;User Id=sa;Password=<Your-SQL-Server-Password>;Database=LibraryManagement;Encrypt=True;TrustServerCertificate=True;"
  },

  "JWT": {
    "Token": "your token"
  }
}

```

### 3. Restore Dependencies

```bash

dotnet restore
```

### 4. Apply Migration and seed the Data

```bash
dotnet ef database update

```

### 5. Run the application

```bash
dotnet run
```
