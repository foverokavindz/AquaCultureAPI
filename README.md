# AquaCulture API

A RESTful backend for managing fish farms and their crew, built with **ASP.NET Core 8** following **N-Tier Architecture** principles.

---

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database](#database)
- [API Endpoints](#api-endpoints)
- [Image Upload](#image-upload)
- [Other Notes](#other-notes)

---

## Overview

AquaCulture API allows clients to:

- Register and manage fish farms with GPS coordinates, cage count, barge status, and a cover image
- Register workers with personal info, crew role, and certification date
- Assign workers to farms with a role per assignment
- Search and filter farms and workers with server-side sorting
- Upload images securely via Cloudinary signed upload

---

## Architecture

The solution follows **Clean Architecture** with four separate C# projects. Dependencies only flow inward — outer layers depend on inner layers, never the reverse.

```
AquaCulture.API           →  Controllers, DTOs, Middleware
AquaCulture.Application   →  Services, Interfaces, Business Logic
AquaCulture.Infrastructure →  Repositories, EF Core, Cloudinary
AquaCulture.Domain        →  Entities, Enums, Value Objects
```

---

## Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core 8 | REST API framework |
| Entity Framework Core 8 | ORM and database migrations |
| PostgreSQL | Relational database |
| Npgsql | PostgreSQL driver for EF Core |
| Cloudinary | Image CDN and storage |
| Swagger / OpenAPI | API documentation |

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (running locally or remote)
- [Cloudinary account](https://cloudinary.com) (free tier is sufficient)

### 1. Clone the repository

```bash
git clone https://github.com/your-username/aquaculture-api.git
cd aquaculture-api
```

### 2. Set up configuration

Create `appsettings.Development.json` inside `AquaCulture.API/` and add your credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=aquaculture;Username=postgres;Password=yourpassword"
  },
  "Cloudinary": {
    "CloudName": "your_cloud_name",
    "ApiKey": "your_api_key",
    "ApiSecret": "your_api_secret",
    "BaseUrl": "https://api.cloudinary.com/v1_1"
  }
}
```

> ⚠️ Never commit `appsettings.Development.json` — it is already in `.gitignore`.

### 3. Run database migrations

```bash
dotnet ef database update --project AquaCulture.Infrastructure --startup-project AquaCulture.API
```

### 4. Run the API

```bash
dotnet run --project AquaCulture.API
```

The API will be available at:
- `https://localhost:7016`
- `http://localhost:5180`

Swagger UI: `https://localhost:7016/swagger`

---

## Configuration

| Key | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | PostgreSQL connection string |
| `Cloudinary:CloudName` | Your Cloudinary cloud name |
| `Cloudinary:ApiKey` | Your Cloudinary API key |
| `Cloudinary:ApiSecret` | Your Cloudinary API secret (never expose this) |
| `Cloudinary:BaseUrl` | Cloudinary upload base URL |

---
```
💡 Quick Note

For testing purpose all the API creds are given in the `appsettings.Development.json` file
```


## Database

### Schema

**fish_farm**

| Column | Type | Notes |
|---|---|---|
| Id | UUID | Primary key |
| Name | VARCHAR(200) | Required |
| Latitude | DECIMAL(9,4) | Required |
| Longitude | DECIMAL(9,4) | Required |
| NoOfCages | INT | Required, min 0 |
| HasBarge | BOOLEAN | Default false |
| PictureUrl | VARCHAR(500) | Optional |
| IsDeleted | BOOLEAN | Default false, soft delete |

**worker**

| Column | Type | Notes |
|---|---|---|
| Id | UUID | Primary key |
| FishFarmId | UUID | Foreign key, nullable |
| Name | VARCHAR(200) | Required |
| ProfileImageUrl | VARCHAR(500) | Optional |
| Age | INT | Required |
| Email | VARCHAR(200) | Required, unique index |
| Position | VARCHAR | Enum string: CEO / Captain / Worker |
| CertifiedUntil | DATE | Required |
| IsDeleted | BOOLEAN | Default false, soft delete |

### Key constraints

- GPS coordinates (`Latitude` + `Longitude`) have a **unique composite index** — two farms cannot share the same location
- `Email` has a **unique index** — no duplicate workers
- Deleting a farm sets `FishFarmId = null` on all its workers (SetNull behavior)
- All queries automatically exclude soft-deleted records via EF Core `HasQueryFilter`

### Migrations

```bash
# create a new migration
dotnet ef migrations add MigrationName --project AquaCulture.Infrastructure --startup-project AquaCulture.API

# apply migrations
dotnet ef database update --project AquaCulture.Infrastructure --startup-project AquaCulture.API

# remove last migration (if not applied)
dotnet ef migrations remove --project AquaCulture.Infrastructure --startup-project AquaCulture.API
```

---

## API Endpoints

All responses follow a consistent shape:

```json
{
  "success": true,
  "data": { },
  "message": "Success",
  "error": null,
  "timestamp": "2026-05-10T12:00:00Z"
}
```

### Fish Farm

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/FishFarm` | Get all farms with workers |
| `GET` | `/api/FishFarm/{id}` | Get farm by ID with workers |
| `GET` | `/api/FishFarm/search` | Search and filter farms |
| `POST` | `/api/FishFarm` | Create farm and assign workers |
| `PUT` | `/api/FishFarm/{id}` | Update farm and reassign workers |
| `DELETE` | `/api/FishFarm/{id}` | Soft delete farm, unassign workers |

**Search query params:**

| Param | Type | Description |
|---|---|---|
| `searchTerm` | string | Filter by farm name |
| `hasBarge` | bool | Filter by barge availability |
| `minAvailableCages` | int | Minimum cage count |
| `maxAvailableCages` | int | Maximum cage count |
| `sortBy` | string | `name_asc`, `name_desc`, `cages_asc`, `cages_desc` |

### Worker

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Worker` | Get all workers with farm info |
| `GET` | `/api/Worker/{id}` | Get worker by ID |
| `GET` | `/api/Worker/farm/{farmId}` | Get workers by farm |
| `GET` | `/api/Worker/search` | Search and filter workers |
| `POST` | `/api/Worker` | Create worker |
| `PUT` | `/api/Worker/{id}` | Update worker |
| `DELETE` | `/api/Worker/{id}` | Soft delete worker |

**Search query params:**

| Param | Type | Description |
|---|---|---|
| `searchTerm` | string | Filter by worker name |
| `position` | string | Filter by crew role |
| `isAssigned` | bool | `true` = assigned to farm, `false` = unassigned |
| `sortBy` | string | `name_asc`, `name_desc`, `age_asc`, `age_desc` |

### Image

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Image/sign` | Get Cloudinary signed upload signature |
| `DELETE` | `/api/Image/{publicId}` | Delete image from Cloudinary |

---

## Image Upload

Image uploads use the **Cloudinary Signed Upload** method.

**Flow:**

```
1. Client  →  GET /api/Image/sign          (request signature)
2. Server  →  generate HMAC signature       (using API secret — never exposed)
3. Client  →  POST file + signature to Cloudinary CDN directly
4. Client  →  receives secure_url + public_id
5. Client  →  include URL in farm/worker create or update request
```

- Each signature is time-limited (expires after 1 hour)
- The server never handles binary file data — reduces load and attack surface

---

## Other Notes

**Soft delete** — Records are never permanently deleted. `IsDeleted = true` is set instead, and `HasQueryFilter` on the DbContext automatically excludes them from all queries.

**GeoLocation as a value object** — Latitude and Longitude are wrapped in a `GeoLocation` class that validates 4 decimal precision in the constructor.

**One worker = one farm** — A worker can only be assigned to one farm at a time. Attempting to assign an already-assigned worker returns `400 Bad Request`.

**GPS uniqueness** — A unique composite index on `(Latitude, Longitude)` prevents two farms from being registered at the same coordinates.

**Atomic transactions** — When creating or updating a farm with worker assignments, all database changes (farm + worker updates) are saved in a single `SaveChangesAsync` call. If anything fails, nothing is committed.

**Repository pattern** — Generic `Repository<T>` handles common CRUD.

---

## Error Handling

All unhandled exceptions are caught by `GlobalExceptionMiddleware` and returned as structured JSON:

```json
{
  "success": false,
  "message": "Farm with id ... not found",
  "data": null,
  "error": "...",
  "timestamp": "2026-05-10T12:00:00Z"
}
```

| Exception | HTTP Status |
|---|---|
| `KeyNotFoundException` | 404 Not Found |
| `InvalidOperationException` | 400 Bad Request |
| `ArgumentException` | 400 Bad Request |
| Any other exception | 500 Internal Server Error |