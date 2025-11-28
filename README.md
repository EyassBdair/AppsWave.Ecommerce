# AppsWave E-Commerce API

A complete RESTful API for a Mini E-Commerce system built with ASP.NET Core using Clean Architecture principles.

## 📋 Table of Contents

- [API Documentation](#api-documentation)
- [Using the API](#using-the-api)
- [Postman Collection](#postman-collection)
- [Architecture](#architecture)
- [Features](#features)


📁 **Postman Directory**: [AppsWave.Ecommerce.Shared/Postman](./AppsWave.Ecommerce/AppsWave.Ecommerce.Shared/Postman)

## 📚 API Documentation

### Base URL

```
https://localhost:44310

```

### Authentication

All endpoints (except Register and Login) require JWT authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-token>
```

### Endpoints Overview

#### 🔐 Authentication (`/api/Auth`)

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/api/Auth/register` | Register a new user | Public |
| POST | `/api/Auth/login` | Login and get JWT token | Public |

#### 📦 Products (`/api/Products`)

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/api/Products/getAll` | Get paginated products | Admin, Visitor |
| POST | `/api/Products/getById` | Get product by ID | Admin, Visitor |
| POST | `/api/Products/createOrUpdate` | Create or update product | Admin |
| POST | `/api/Products/delete` | Delete product (soft delete) | Admin |

#### 🧾 Invoices (`/api/Invoices`)

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/api/Invoices` | Create new invoice | Visitor |
| POST | `/api/Invoices` | Get invoice by ID | Admin, Visitor |
| POST | `/api/Invoices` | Get all invoices | Admin |
| POST | `/api/Invoices` | Update invoice | Admin |

## 🔌 Using the API

### 1. Register a New User

**Request:**
```http
POST /api/Auth/register
Content-Type: application/json

{
  "fullName": "John Doe",
  "email": "john@example.com",
  "username": "johndoe",
  "password": "Password123"
}
```

**Response:**
```json
"User registered successfully."
```

### 2. Login

**Request:**
```http
POST /api/Auth/login
Content-Type: application/json

{
  "username": "johndoe",
  "password": "Password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9..."
}
```

**Save this token for subsequent requests!**

### 3. Get All Products

**Request:**
```http
POST /api/Products/getAll
Content-Type: application/json
Authorization: Bearer <your-token>

{
  "page": 1,
  "pageSize": 10
}
```

**Response:**
```json
{
  "totalItems": 15,
  "page": 1,
  "pageSize": 10,
  "items": [
    {
      "id": 1,
      "arabicName": "منتج 1",
      "englishName": "Product 1",
      "price": 100.50
    }
  ]
}
```

### 4. Get Product by ID

**Request:**
```http
POST /api/Products/getById
Content-Type: application/json
Authorization: Bearer <your-token>

{
  "id": 1
}
```

### 5. Create or Update Product (Admin Only)

**Create New Product:**
```http
POST /api/Products/createOrUpdate
Content-Type: application/json
Authorization: Bearer <admin-token>

{
  "id": 0,
  "arabicName": "منتج جديد",
  "englishName": "New Product",
  "price": 99.99
}
```

**Update Existing Product:**
```http
POST /api/Products/createOrUpdate
Content-Type: application/json
Authorization: Bearer <admin-token>

{
  "id": 1,
  "arabicName": "منتج محدث",
  "englishName": "Updated Product",
  "price": 149.99
}
```

### 6. Delete Product (Admin Only)

**Request:**
```http
POST /api/Products/delete
Content-Type: application/json
Authorization: Bearer <admin-token>

{
  "id": 1
}
```

### 7. Create Invoice (Visitor)

**Request:**
```http
POST /api/Invoices
Content-Type: application/json
Authorization: Bearer <visitor-token>

{
  "details": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 2,
      "quantity": 1
    }
  ]
}
```

**Response:**
```json
{
  "message": "Invoice created successfully.",
  "invoice": {
    "id": 1,
    "date": "2024-01-15T10:30:00Z",
    "userId": 1,
    "totalAmount": 250.00,
    "details": [
      {
        "productId": 1,
        "price": 100.00,
        "quantity": 2
      }
    ]
  }
}
```

### 8. Get Invoice by ID

**Request:**
```http
POST /api/Invoices
Content-Type: application/json
Authorization: Bearer <your-token>

{
  "id": 1
}
```

**Note:** Visitors can only see their own invoices. Admins can see all invoices.

### 9. Get All Invoices (Admin Only)

**Request:**
```http
POST /api/Invoices
Content-Type: application/json
Authorization: Bearer <admin-token>
```

### 10. Update Invoice (Admin Only)

**Request:**
```http
POST /api/Invoices
Content-Type: application/json
Authorization: Bearer <admin-token>

{
  "id": 1,
  "details": [
    {
      "productId": 1,
      "quantity": 3
    }
  ]
}
```

## 📮 Postman Collection

A complete Postman collection is included in the repository:
📁 **Postman Directory**: [AppsWave.Ecommerce.Shared/Postman](./AppsWave.Ecommerce/AppsWave.Ecommerce.Shared/Postman)

### How to Import the Collection

1. Open **Postman**
2. Click **Import** button (top left)
3. Select **File** tab
4. Choose `AppsWave.Ecommerce.postman_collection.json`
5. Click **Import**

### Collection Structure

The collection includes:

#### 🔐 Authentication
- **Register** - Create a new user account
- **Login** - Get JWT authentication token

#### 📦 Products
- **Get All Products** - Get paginated list of products
- **Get Product by ID** - Get single product details
- **Create or Update Product** - Create new or update existing product
- **Delete Product** - Soft delete a product

#### 🧾 Invoices
- **Create Invoice** - Create a new invoice (Visitor)
- **Get Invoice by ID** - Get invoice details
- **Get All Invoices** - Get all invoices (Admin)
- **Update Invoice** - Update invoice details (Admin)

### Using the Collection

1. **Set Base URL Variable:**
   - The collection uses `{{base_url}}` variable
   - Set it to: `https://localhost:44310`

2. **Get Authentication Token:**
   - First, run the **Register** request to create a user
   - Then run the **Login** request
   - Copy the token from the response
   - Go to the **Authorization** tab in other requests
   - Select **Bearer Token** and paste your token

3. **Test Endpoints:**
   - All requests are pre-configured with proper headers
   - Just update the request body as needed
   - Click **Send** to test

### Environment Variables

You can create a Postman Environment with:
- `base_url`: `https://localhost:44310`
- `token`: (will be set after login)

## 🏗️ Architecture

The project follows **Clean Architecture** principles with the following layers:

- **Domain**: Contains entities and domain models (User, Product, Invoice, InvoiceDetail)
- **Shared**: Contains DTOs, Validators (FluentValidation), and shared models
- **Application**: Contains business logic, interfaces (IProductService, IInvoiceService, IAuthService), and services
- **Infrastructure**: Contains data access (DbContext, DbInitializer), password hashing (BCrypt), and external dependencies
- **WebApi**: Contains controllers, API configuration, and Swagger setup

## ✨ Features

- ✅ **User Registration & Authentication** using JWT (JSON Web Tokens)
- ✅ **Password Hashing** using BCrypt (secure password storage)
- ✅ **Role-based Authorization** (Admin, Visitor)
- ✅ **Product Management** (Create, Read, Update, Delete with Soft Delete)
- ✅ **Invoice Generation** with automatic total calculation
- ✅ **Pagination** for product listings
- ✅ **Data Validation** using FluentValidation
- ✅ **SQL Server Database** with Entity Framework Core
- ✅ **Swagger UI** for API documentation
- ✅ **Unit Tests** using xUnit
- ✅ **Clean Architecture** implementation
- ✅ **All APIs use POST method** for consistency



**Built with Eyass Bdair **
