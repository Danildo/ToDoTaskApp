# Todo Task Manager

## Developer's Note

During the development of this project, I utilized (AI) artificial intelligence as a support tool to enhance the Documentation and test of my work.

## Overview

This project is a simple Todo Task Manager built with ASP.NET Core. It allows users to manage their to-do tasks by setting their status to "In Progress", "Pending", or "Done". Additionally, users can clear all tasks that are marked as "Done".

## Features

- **Set Task Status**: Update the status of a specific to-do task to "In Progress", "Pending", or "Done".
- **Clear Done Tasks**: Remove all tasks that are marked as "Done".
- **CRUD Operations**: Create, read, update, and delete to-do tasks.

## Dependency Injection

Dependency Injection (DI) is used in this project to achieve loose coupling between the controllers and the repository. By injecting dependencies, we can easily swap out implementations, making the code more modular, testable, and maintainable. ASP.NET Core provides built-in support for DI, which simplifies the process of managing dependencies.

## Database Migrations

This project uses Entity Framework Core for database interactions. Migrations are used to keep the database schema in sync with the application's data model. Migrations allow you to evolve your database schema over time in a consistent and controlled way.

### Migration Middleware

To ensure that the database is always up-to-date with the latest schema changes, a custom middleware has been added. This middleware automatically applies any pending migrations whenever the application starts. This ensures that the database schema mirrors the application's data model, reducing the risk of runtime errors due to schema mismatches.

```csharp
public class MigrationMiddleware
{
    private readonly RequestDelegate _next;

    public MigrationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ToDoTaskContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        await _next(context);
    }
}
```

## Benefits of Using Modular Architecture

Modular architecture divides the application into smaller, self-contained modules, each responsible for a specific functionality. This approach offers several advantages:

- **Improved Readability**: Modular code is well-organized, making it easier to understand and navigate. Each module has a clear purpose and responsibility.
- **Easier Maintenance**: When a module needs fixing or updating, you can focus on that specific module without affecting the rest of the application. This reduces the risk of introducing bugs and makes the codebase more maintainable.
- **Parallel Development**: Multiple developers or teams can work on different modules simultaneously without interfering with each other. This speeds up the development process and improves collaboration.
- **Reusability**: Modules can be reused across different parts of the application or even in other projects, saving development time and effort.
- **Scalability**: Modular architecture allows for easier scaling of the application. You can add new features or modify existing ones without disrupting the entire system.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or any other C# IDE

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Danildo/ToDoTaskApp.git
   cd ToDoTaskApp/src/ToDoTaskApplication
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the project**:
   ```bash
   dotnet build
   ```

### Running the Application

1. **Run the application**:
   ```bash
   dotnet run
   ```

2. **Open your browser** and navigate to `http://localhost:5157` to see the application in action.

### Running Tests

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Danildo/ToDoTaskApp.git
   cd ToDoTaskApp/src/ToDoTaskApplication.Test
   ```

2. **Run the tests**:
   ```bash
   dotnet test
   ```

## Project Structure

- **Controllers**: Contains the logic for handling HTTP requests and responses.
- **Repositories**: Contains the logic for data access and manipulation.
- **Models**: Contains the data models used in the application.
- **Views**: Contains the Razor views for rendering the UI.

## Design Decisions and Architectural Choices

### Framework and Frontend

- **ASP.NET Core**: Chosen for its performance, scalability, and cross-platform capabilities.
- **MVC with Razor**: Provides a clean separation of concerns and a powerful templating engine for building dynamic web pages.

### Backend and API

- **API for CRUD Operations**: The backend API handles creating, reading, updating, and deleting tasks. This separation allows for a clear and maintainable codebase.
- **SOLID Principles**: Applied to ensure the application is scalable, maintainable, and easy to understand.

### Database

- **Local Database**: Used for storing tasks. This setup is suitable for development and testing environments.

### Dependency Injection

- **DI**: Used to inject the repository into the controller, promoting loose coupling and making the application more testable.

### ORM

- **Entity Framework**: Used for database interactions, providing a robust and easy-to-use ORM for managing data.
