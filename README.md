# BrtzStore Project

A full-stack e-commerce web application and RESTful API built with **.NET 9**, **ASP.NET Core**, **MongoDB**, and **Bootstrap 5**.

---

## 📌 Architecture Overview

The solution consists of two primary projects:

1. **`BrzSaleApi`** (Backend REST API)
   * ASP.NET Core Web API with API Versioning (`v1.0` and `v2.0`).
   * **Database:** MongoDB (via MongoDB.Driver).
   * **Authentication:** JWT Bearer Token support.
   * **Documentation:** Interactive OpenAPI / Swagger UI at `https://localhost:7069/swagger`.
   * **Endpoints:** Full CRUD operations for Products (`GET`, `POST`, `PUT`, `DELETE`).

2. **`BrzSaleWebApp`** (Frontend Web Application - BrtzStore)
   * ASP.NET Core Razor Pages with Bootstrap 5 responsive layout.
   * **Authentication:** ASP.NET Core Identity with MongoDB persistence (`AspNetCore.Identity.MongoDbCore`).
   * **Store Dashboard:** Interactive Product Management interface (Card view & Table CRUD view) directly connected to the REST API.
   * **Sample Seeder:** One-click demo data population for testing.

---

## 🚀 Prerequisites

Ensure you have the following installed on your machine:

* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [MongoDB Community Server](https://www.mongodb.com/try/download/community) running locally on port `27017` (`mongodb://localhost:27017`).
* Windows PowerShell (Windows 10/11).

---

## ⚡ How to Run

### Option 1: Automated Script (Recommended)

From the project root directory, run the included PowerShell automation script:

```powershell
.\run.ps1
```

This script will:
1. Launch **BrzSaleApi** in a separate terminal.
2. Wait 4 seconds for the API and HTTPS endpoints to initialize.
3. Launch **BrzSaleWebApp** in a separate terminal.

### Option 2: Manual Terminal Commands

Open two separate terminals from the root directory:

**Terminal 1 (Backend API):**
```powershell
dotnet run --project BrzSaleApi\BrzSaleApi
```

**Terminal 2 (Frontend Web App):**
```powershell
dotnet run --project BrzSaleApi\BrzSaleWebApp
```

---

## 🌐 Application URLs

| Service | Protocol | URL | Description |
| :--- | :--- | :--- | :--- |
| **BrtzStore Web App** | HTTPS | [https://localhost:7047](https://localhost:7047) | Main store frontend & product management |
| **BrtzStore Web App** | HTTP | `http://localhost:5086` | Alternative HTTP port |
| **BrzSaleApi Swagger** | HTTPS | [https://localhost:7069/swagger](https://localhost:7069/swagger) | Interactive API documentation & testing |
| **BrzSaleApi Root** | HTTPS | [https://localhost:7069/](https://localhost:7069/) | Automatically redirects to `/swagger` |

---

## 🛠️ API Endpoints Summary

### Products (`v1.0`)

* `GET /api/v1.0/Products` — Retrieve paginated products with optional filtering (`minPrice`, `maxPrice`, `brand`, `model`, `sortBy`, `sortOrder`).
* `GET /api/v1.0/Products/{id}` — Get single product details by 24-character ObjectId.
* `POST /api/v1.0/Products` — Create a new product.
* `PUT /api/v1.0/Products/{id}` — Update an existing product.
* `DELETE /api/v1.0/Products/{id}` — Delete a product.
* `DELETE /api/v1.0/Products/Delete?ids=...` — Batch delete multiple products.

---

## 🔒 Security & Credentials

* **HTTPS Developer Certificates:** If prompted for SSL trust, run:
  ```powershell
  dotnet dev-certs https --trust
  ```
* **Identity Management:** User registration and password hashing handled through ASP.NET Core Identity with MongoDB document store.
* **Sensitive Files:** Temporary compilation files, binaries (`bin/`, `obj/`), user settings (`*.user`), and environment variables are ignored via `.gitignore`.
