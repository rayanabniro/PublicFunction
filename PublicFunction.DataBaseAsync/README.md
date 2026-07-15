# PublicFunction.DataBaseAsync

A comprehensive, **fully asynchronous** C# library for interacting with various SQL and NoSQL databases. This library is designed to improve scalability and performance in high-concurrency applications by leveraging the `async/await` pattern for all I/O operations.

## 🚀 Features

- **Asynchronous by Design**: All methods are async, ensuring non-blocking database operations.
- **Multi-Database Support**: Provides consistent, async-first managers for:
  - **Microsoft SQL Server** (Async `SQLService`)
  - **Redis** (Async `RedisManager` for key-value, hashes, lists, and sets)
  - **MongoDB**
  - **MySQL**
  - **Oracle**
  - **RabbitMQ**
- **Centralized Configuration**: Simple configuration via `appsettings.json`.
- **Dependency Injection Ready**: Built to work seamlessly with .NET Core's DI container.

## 📦 Installation

To install the package via NuGet, run the following command in your project:

```bash
dotnet add package PublicFunction.DataBaseAsync