# BrtzStore Project

A full-stack e-commerce web application and RESTful API built with **.NET 9**, **ASP.NET Core**, **MongoDB**, and **Bootstrap 5**.

---

## 📌 Architecture Overview

The solution consists of three primary projects:

1. **`BrzSaleApi`** (Backend REST API)
   * ASP.NET Core Web API with API Versioning (`v1.0` and `v2.0`).
   * **Database:** MongoDB (via MongoDB.Driver).
   * **Authentication:** JWT Bearer Token support.
   * **Documentation:** Interactive OpenAPI / Swagger UI at `https://localhost:7069/swagger`.
   * **Endpoints:** Full CRUD operations for Products (`GET`, `POST`, `PUT`, `DELETE`).

2. **`BrzSaleWebApp`** (Frontend Web Application - BrtzStore)
   * ASP.NET Core Razor Pages with Bootstrap 5 responsive layout.
   * **Authentication & Authorization:** ASP.NET Core Identity with MongoDB persistence (`AspNetCore.Identity.MongoDbCore`).
   * **Role & Session-Based Access Control:**
     * **Guests (Unauthenticated):** Read-only mode to browse the product catalog and view cards.
     * **Authenticated Users:** Full management capabilities — Create, Edit, Delete products, Table CRUD view, and sample data population.
   * **Sample Seeder:** One-click demo data population for testing (restricted to logged-in users).

3. **`BrzSaleApi.Tests`** (Unit Testing Suite)
   * Built with **xUnit**, **Moq**, and **Microsoft.AspNetCore.Mvc.Testing**.
   * Covers model validation (`ProductModelTests`) and controller endpoints (`ProductsControllerTests`).

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

## 🧪 Testing & CI/CD

### Running Unit Tests Locally

Execute all unit tests across the solution:

```powershell
dotnet test BrzSaleApi\BrzSaleApi.sln
```

### Continuous Integration (GitHub Actions)

A CI workflow is configured in [`.github/workflows/ci.yml`](.github/workflows/ci.yml) that triggers automatically on every `push` and `pull_request` to the `main` branch:
* Checks out repository code.
* Sets up .NET 9 SDK.
* Restores all NuGet dependencies.
* Builds the solution in `Release` configuration.
* Executes the complete test suite.

---

## 🌐 Application URLs

| Service | Protocol | URL | Description |
| :--- | :--- | :--- | :--- |
| **BrtzStore Web App** | HTTPS | [https://localhost:7047](https://localhost:7047) | Main store frontend & product catalog |
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

## 🔒 Security & Access Control

* **Catalog Permissions:** Unauthenticated guests can view the product catalog in read-only mode. All write actions (Add, Edit, Delete, Seed data) require an active user session.
* **HTTPS Developer Certificates:** If prompted for SSL trust, run:
  ```powershell
  dotnet dev-certs https --trust
  ```
* **Identity Management:** User registration, password hashing, and cookie session authentication handled through ASP.NET Core Identity with MongoDB document store.
* **Sensitive Files:** Temporary compilation files, binaries (`bin/`, `obj/`), user settings (`*.user`), and environment variables are excluded via `.gitignore`.
