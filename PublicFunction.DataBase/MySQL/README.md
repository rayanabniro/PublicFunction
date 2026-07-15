### Overview

-   **Interface (`IMySQLService`)**: Defines the contract for the MySQL service, outlining methods for executing queries, managing parameters, and performing CRUD operations.
-   **Class (`MySQLService`)**: Implements the `IMySQLService` interface, providing concrete implementations for interacting with a MySQL database using `MySql.Data.MySqlClient`.

### Prerequisites

1.  **MySQL Connector/NET**: Ensure that the `MySql.Data` package is installed in your project. You can install it via NuGet:
    
```bash
    Install-Package MySql.Data
 ```
 **Connection Strings**: Configure your connection strings appropriately in your application's configuration file (e.g., `appsettings.json`).

### Explanation of the `MySQLService` Class

1.  **Constructor**:
    
    -   Initializes the `MySqlConnection` using the connection string retrieved from the configuration.
2.  **StoredProcedureName Property**:
    
    -   Gets or sets the name of the stored procedure to execute.
    -   When setting, it initializes the `_mysqlCommand` with the specified stored procedure name and sets the command type to `StoredProcedure`.
3.  **AddParameter Methods**:
    
    -   **`AddParameter(string name, MySqlDbType dbType, object data)`**: Adds a parameter with a specified `MySqlDbType`.
    -   **`AddParameter(string name, object data)`**: Adds a parameter without specifying the `MySqlDbType`; the type is inferred from the data.
    -   **`AddParametersFromModel<T>(T model)`**: Adds multiple parameters from a model that inherits from `MySQLStoredProcedureModel`. This method iterates over the properties of the model and adds them as parameters.
4.  **Execute Method**:
    
    -   Executes a non-query stored procedure (such as INSERT, UPDATE, DELETE).
    -   Opens the connection, executes the command, and then closes the connection.
5.  **Select Methods**:
    
    -   **`Select()`**: Executes a SELECT query and returns the results as a `DataTable`.
    -   **`SelectList()`**: Executes a SELECT query and returns the results as a list of dictionaries for easier data manipulation.
    -   **`SelectModel<T>()`**: Executes a SELECT query and maps the first result to a model that inherits from `MySQLDataModel`.
6.  **MultiSelect Methods**:
    
    -   **`MultiSelect()`**: Executes multiple SELECT queries and returns the results as a `DataSet`.
    -   **`MultiSelectList()`**: Executes multiple SELECT queries and returns the results as a list of lists of dictionaries.
    -   **`MultiSelectModel<T>(string[] tableNames)`**: Executes multiple SELECT queries and maps the results to a model with multiple tables.
7.  **CRUD Operations**:
    
    -   **`Insert<T>(string tableName, T model)`**: Inserts a new record into the specified table using the provided model.
    -   **`Update<T>(string tableName, string id, T model)`**: Updates an existing record in the specified table identified by `ID` using the provided model.
    -   **`Delete(string tableName, string id)`**: Deletes a record from the specified table identified by `ID`.
8.  **Error Handling**:
    
    -   Each method includes try-catch blocks to handle exceptions and provide meaningful error messages.
9.  **Parameter Prefix**:
    
    -   MySQL uses `@` as the prefix for parameters (e.g., `@ParameterName`), which is reflected in the `AddParametersFromModel` and CRUD methods.


### Example Usage

Here's how you can utilize the `MySQLService` class in your application:
```csharp
using System;
using System.Data;
using Microsoft.Extensions.Configuration;

public class EmployeeModel : MySQLDataModel
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // Setup configuration (e.g., from appsettings.json)
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        IMySQLService mySQLService = new MySQLService(configuration);

        try
        {
            // Example: Insert a new employee
            var newEmployee = new EmployeeModel
            {
                ID = 1,
                Name = "John Doe",
                Department = "HR"
            };
            bool insertResult = mySQLService.Insert("Employees", newEmployee);
            Console.WriteLine($"Insert successful: {insertResult}");

            // Example: Select employees
            string selectQuery = "SELECT * FROM Employees WHERE Department = @Department";
            mySQLService.StoredProcedureName = selectQuery;
            mySQLService.AddParameter("@Department", "HR");
            DataTable employees = mySQLService.Select();
            foreach (DataRow row in employees.Rows)
            {
                Console.WriteLine($"ID: {row["ID"]}, Name: {row["Name"]}, Department: {row["Department"]}");
            }

            // Example: Update an employee
            var updatedEmployee = new EmployeeModel
            {
                Name = "John Smith",
                Department = "Finance"
            };
            bool updateResult = mySQLService.Update("Employees", "1", updatedEmployee);
            Console.WriteLine($"Update successful: {updateResult}");

            // Example: Delete an employee
            bool deleteResult = mySQLService.Delete("Employees", "1");
            Console.WriteLine($"Delete successful: {deleteResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Operation failed: {ex.Message}");
        }
    }
}

```

### Configuration (`appsettings.json`)

Ensure that your connection strings are properly defined in your configuration file:

```json
{
  "PublicFunction": {
    "DataBase": {
      "MySQL":  {
          "ConnectionString": "Server=your_server;Database=your_database;User ID=your_username;Password=your_password;SslMode=none;"
      }
    }
  }
}

```

**Note**: Replace `your_server`, `your_database`, `your_username`, and `your_password` with your actual MySQL server details. Adjust `SslMode` as needed based on your security requirements.

### Detailed Explanation

1.  **Connection Management**:
    
    -   The `MySQLService` class ensures that connections are opened and closed appropriately within each method to prevent connection leaks.
2.  **Parameter Ordering**:
    
    -   Unlike some other databases, MySQL allows parameters to be named, so the order is less critical. However, it's still good practice to add parameters in the order they appear in your queries.
3.  **Stored Procedures**:
    
    -   When using stored procedures, ensure that they are defined correctly in the MySQL database and that the parameter names and types match those expected by the procedures.
4.  **Exception Handling**:
    
    -   The class provides detailed exception messages to aid in debugging. In a production environment, consider logging these exceptions instead of throwing them directly.
5.  **Security Considerations**:
    
    -   Avoid hardcoding sensitive information like connection strings. Use secure methods to store and retrieve configuration data, such as environment variables or secure vaults.
6.  **Performance Optimization**:
    
    -   For high-performance applications, consider implementing connection pooling and reusing `MySqlCommand` objects where appropriate.
7.  **Extensibility**:
    
    -   The `MySQLService` class is designed to be extensible. You can add more methods or extend existing ones to cater to additional requirements, such as bulk operations or advanced transaction management.

### Best Practices

-   **Use Parameterized Queries**: Always use parameterized queries to prevent SQL injection attacks. The `AddParameter` methods in the `MySQLService` class facilitate this.
    
-   **Dispose Resources Properly**: Ensure that all database connections and commands are properly disposed of. The use of `using` statements in methods like `Insert`, `Update`, and `Delete` ensures that resources are cleaned up even if exceptions occur.
    
-   **Centralize Configuration**: Store all your connection strings and other configuration details in a centralized configuration file (`appsettings.json`), making it easier to manage and secure.
    
-   **Logging**: Implement logging to capture detailed information about database operations and exceptions. This aids in monitoring and debugging.
    
-   **Error Handling**: Handle exceptions gracefully and provide meaningful error messages. Avoid exposing sensitive information in error messages, especially in production environments.







