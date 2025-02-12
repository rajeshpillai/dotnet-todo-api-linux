# Todo API (C# .NET 9 + SQLite)

This is a simple REST API for a To-Do Application built using C# .NET 9 with Entity Framework Core and SQLite. It provides CRUD operations for managing tasks.

1. Prerequisites

Ensure you have the following installed:

.NET SDK 8 (Download)

SQLite (included with EF Core)

To verify .NET installation, run:

```dotnet --version```

2. Setup the Project

Clone the Repository

```git clone https://github.com/your-repo/todo-api.git
cd todo-api
```

Install Required Packages

```
dotnet restore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet tool install --global dotnet-ef
```

3. Database Migrations & Seeding

Generate Migrations

```dotnet ef migrations add InitialCreate```

Apply Migrations & Seed Data

```dotnet ef database update```

4. Run the API

Start the application:

```dotnet run```

By default, the API runs at http://localhost:5000.

5. API Endpoints

## Get All Todos

GET /api/todos

## Get a Todo by ID

GET /api/todos/{id}

## Create a Todo

```POST /api/todos
Content-Type: application/json
{
  "title": "New Task",
  "tagIds": [1, 2]
}
```

## Update a Todo

```PUT /api/todos/{id}
Content-Type: application/json
{
  "id": 1,
  "title": "Updated Task",
  "isCompleted": true
}
```

## Delete a Todo

```
DELETE /api/todos/{id}
```

## Toggle Task Completion

```
PATCH /api/todos/{id}/toggle
```

## Assign tags to Todo
```
POST /api/todos/{id}/tags
Content-Type: application/json
[
  1, 2, 3
]
```


6. Docker Setup (Optional)

Build Docker Image

docker build -t todo-api .

Run Docker Container

docker run -p 5000:5000 todo-api

7. Deployment

To deploy on Azure, AWS, or DigitalOcean, use Docker or CI/CD pipelines.

✅ The API is now ready! 🚀 Feel free to contribute or suggest improvements.

