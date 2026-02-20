# Restaurant Management System

A full-stack restaurant management web application built with React and ASP.NET Core Web API.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | React 18 |
| Backend | ASP.NET Core 6 Web API (C#) |
| Database | MySQL 8 |
| Auth | JWT (JSON Web Token) |
| API Docs | Swagger / OpenAPI |

## Features

- Browse food menu by category
- Add items to cart and place orders
- JWT-based user authentication (register / login)
- RESTful API with Swagger documentation
- CORS-enabled for frontend-backend communication

## Project Structure

```
RestaurantManagement/
├── BE/
│   └── RPWebApi/          # ASP.NET Core Web API
│       ├── Controllers/   # API endpoints
│       ├── Models/        # Data models
│       ├── Services/      # Business logic & DB access
│       └── Dto/           # Data Transfer Objects
└── FE/
    └── src/
        └── components/    # React components
            ├── Cart/
            ├── Meals/
            ├── Layout/
            └── UI/
```

## API Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/Category` | Get all categories | No |
| GET | `/Food` | Get all food items | No |
| GET | `/Food/{categoryId}` | Get food by category | No |
| POST | `/User` | Register new user | No |
| POST | `/User/login` | Login & get JWT token | No |
| GET | `/FoodOrder` | Get all orders | Yes |
| POST | `/FoodOrder` | Create new order | Yes |
| GET | `/FoodOrderItem` | Get order items | Yes |
| POST | `/FoodOrderItem` | Add order item | Yes |

Full API documentation available at `/swagger` when running locally.

## Getting Started (Local)

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- MySQL 8

### Database Setup

```sql
CREATE DATABASE restaurant;
```

Import your schema and seed data.

### Backend

```bash
cd BE/RPWebApi

# Copy example config and fill in your values
cp appsettings.example.json appsettings.json
# Edit appsettings.json with your DB password and JWT secret key

dotnet run
# API runs at http://localhost:5555
# Swagger UI at http://localhost:5555/swagger
```

### Frontend

```bash
cd FE

# Create environment file
echo REACT_APP_API_URL=http://localhost:5555 > .env

npm install
npm start
# App runs at http://localhost:3000
```

## Environment Variables

### Backend (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=restaurant;uid=root;Pwd=YOUR_PASSWORD;"
  },
  "JwtSettings": {
    "SecretKey": "YOUR_SECRET_KEY_MIN_64_CHARACTERS",
    "Issuer": "RPWebApi",
    "Audience": "RPWebApiClient",
    "ExpiryInHours": 24
  }
}
```

### Frontend (`.env`)

```
REACT_APP_API_URL=http://localhost:5555
```

## Security Notes

- `appsettings.json` and `.env` are excluded from git (see `.gitignore`)
- Use `appsettings.example.json` as a template
- JWT secret key must be at least 64 characters (512 bits) for HMACSHA512
