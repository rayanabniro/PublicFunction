### Explanation of the `OracleService` Class

1.  **Constructor**:
    
    -   Initializes the `OdbcConnection` using the connection string retrieved from the configuration.
2.  **StoredProcedureName Property**:
    
    -   Gets or sets the name of the stored procedure to execute.
    -   When setting, it initializes the `_oracleCommand` with the specified stored procedure name and sets the command type to `StoredProcedure`.
3.  **AddParameter Methods**:
    
    -   **`AddParameter(string name, OdbcType dbType, object data)`**: Adds a parameter with a specified `OdbcType`.
    -   **`AddParameter(string name, object data)`**: Adds a parameter without specifying the `OdbcType`; the type is inferred from the data.
    -   **`AddParametersFromModel<T>(T model)`**: Adds multiple parameters from a model that inherits from `OracleStoredProcedureModel`. This method iterates over the properties of the model and adds them as parameters.
4.  **Execute Method**:
    
    -   Executes a non-query stored procedure (such as INSERT, UPDATE, DELETE).
    -   Opens the connection, executes the command, and then closes the connection.
5.  **Select Methods**:
    
    -   **`Select()`**: Executes a SELECT query and returns the results as a `DataTable`.
    -   **`SelectList()`**: Executes a SELECT query and returns the results as a list of dictionaries for easier data manipulation.
    -   **`SelectModel<T>()`**: Executes a SELECT query and maps the first result to a model that inherits from `OracleDataModel`.
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
    
    -   Oracle uses `:` as the prefix for parameters (e.g., `:ParameterName`), which is reflected in the `AddParametersFromModel` and CRUD methods.

### Example Usage

Here's how you can utilize the `OracleService` class in your application:

```csharp
using System;
using System.Data;
using Microsoft.Extensions.Configuration;

public class EmployeeModel : OracleDataModel
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

        IOracleService oracleService = new OracleService(configuration);

        try
        {
            // Example: Insert a new employee
            var newEmployee = new EmployeeModel
            {
                ID = 1,
                Name = "John Doe",
                Department = "HR"
            };
            bool insertResult = oracleService.Insert("Employees", newEmployee);
            Console.WriteLine($"Insert successful: {insertResult}");

            // Example: Select employees
            oracleService.StoredProcedureName = "GetAllEmployees"; // Assuming you have this stored procedure
            DataTable employees = oracleService.Select();
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
            bool updateResult = oracleService.Update("Employees", "1", updatedEmployee);
            Console.WriteLine($"Update successful: {updateResult}");

            // Example: Delete an employee
            bool deleteResult = oracleService.Delete("Employees", "1");
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
      "Oracle": {
        "OracleService": {
          "ConnectionString": "Driver={Oracle in OraClient11g_home1};Dbq=YourOracleDB;Uid=your_username;Pwd=your_password;"
        }
      }
    }
  }
}

```

### Notes and Best Practices

1.  **Connection Management**:
    
    -   The `OracleService` class ensures that connections are opened and closed appropriately within each method to prevent connection leaks.
2.  **Parameter Ordering**:
    
    -   Unlike SQL Server, Oracle often relies on parameter order rather than names. Ensure that parameters are added in the correct order as they appear in the stored procedures or SQL statements.
3.  **Stored Procedures**:
    
    -   When using stored procedures, ensure that they are defined correctly in the Oracle database and that the parameter names and types match those expected by the procedures.
4.  **Exception Handling**:
    
    -   The class provides detailed exception messages to aid in debugging. In a production environment, consider logging these exceptions instead of throwing them directly.
5.  **Security Considerations**:
    
    -   Avoid hardcoding sensitive information like connection strings. Use secure methods to store and retrieve configuration data, such as environment variables or secure vaults.
6.  **Performance Optimization**:
    
    -   For high-performance applications, consider implementing connection pooling and reusing `OdbcCommand` objects where appropriate.
7.  **Extensibility**:
    
    -   The `OracleService` class is designed to be extensible. You can add more methods or extend existing ones to cater to additional requirements, such as bulk operations or advanced transaction management.

### Conclusion

This implementation provides a robust and flexible way to interact with an Oracle database using C#.
