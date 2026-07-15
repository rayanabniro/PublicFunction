## PublicFunction.AsyncDataBase

[README.md](https://github.com/rayanabniro/PublicFunction/blob/main/PublicFunction/DataBase/README.md "README.md")

# PublicFunction.AsyncDataBase.SQL

In this **class**, we have infrastructures that can **centrally control** the connection to the **sql database** asynchronously

## Use in .netCore
- in **appsettings.json** define Connection string like this
    ```json
        {
          "PublicFunction": {
            "AsyncDataBase": {
              "SQL": {
                  "ConnectionString": "sql Connection String",
                  "ConnectionStringMirror":"for All Select Class Use this Connection String"
              }
            }
          }
        }
    ```
- in **Program.cs**  Add This dependence
  ```C#
    builder.Services.AddSingleton<IConfiguration>(_ => configuration);
    OR
    builder.Services.AddSingleton<IConfiguration>(sp => {return builder.Configuration;});
  ```
  
- in **Program.cs**  Add This dependence
  ```C#
    builder.Services.AddScoped<PublicFunction.AsyncDataBase.SQLAsync.ISQLServiceAsync, PublicFunction.AsyncDataBase.SQLAsync.SQLServiceAsync>();
  ```
- Suppose you want to use functions in a class, just write the following code in its constructor function
  ```C#
    public class Test
    	{
    		private readonly PublicFunction.AsyncDataBase.SQLAsync.ISQLServiceAsync SQLService;
    		public Test(PublicFunction.AsyncDataBase.SQLAsync.ISQLServiceAsync sqlservice)
    		{
    		    SQLService = sqlservice;
    		}
    	}
  ```

  # SQLServiceAsync Documentation

## Overview
`SQLServiceAsync` is a class that provides **asynchronous** methods to interact with a SQL database using stored procedures and direct SQL commands. It implements the `ISQLServiceAsync` interface.

## Classes and Interfaces

### SQLStoredProcedureModel
A base class for models used in stored procedures.

### SQLDataModel
A base class for data models.

### ISQLServiceAsync Interface
Defines the methods and properties for SQLServiceAsync.

#### Properties
- `string StoredProcedureName { get; set; }`
  - Gets or sets the name of the stored procedure.

#### Methods
- `void AddParameter(string Name, SqlDbType DbType, object Data)`
  - Adds a parameter to the SQL command with a specified name, type, and value.

- `void AddParameter(string Name, object Data)`
  - Adds a parameter to the SQL command with a specified name and value.

- `void AddParameterS<T>(T TModel) where T : SQLStoredProcedureModel`
  - Adds parameters to the SQL command based on the properties of the provided model.

- `Task<bool> AddAsync()`
  - **Asynchronously** executes a non-query SQL command.

- `Task<DataTable> SelectAsync()`
  - **Asynchronously** executes a query and returns the result as a DataTable.

- `Task<List<Dictionary<string, object>>> SelectListAsync()`
  - **Asynchronously** executes a query and returns the result as a list of dictionaries.

- `Task<T> SelectModelAsync<T>() where T : SQLDataModel`
  - **Asynchronously** executes a query and maps the result to a model of type `T`.

- `Task<DataSet> MultiSelectAsync()`
  - **Asynchronously** executes a query and returns the result as a DataSet.

- `Task<List<List<Dictionary<string, object>>>> MultiSelectListAsync()`
  - **Asynchronously** executes a query and returns the result as a list of lists of dictionaries.

- `Task<T> MultiSelectModelAsync<T>(string[] tableNames) where T : SQLDataModel`
  - **Asynchronously** executes a query and maps the result to a model of type `T` based on the specified table names.

- `Task<bool> InsertAsync<T>(string tableName, T Model) where T : SQLDataModel`
  - **Asynchronously** inserts a new record into the specified table using the provided model.

- `Task<bool> UpdateAsync<T>(string tableName, string ID, T Model) where T : SQLDataModel`
  - **Asynchronously** updates an existing record in the specified table using the provided model and ID.

- `Task<bool> DeleteAsync(string tableName, string ID)`
  - **Asynchronously** deletes a record from the specified table using the provided ID.

## Example Usage

```csharp
public class MyStoredProcedureModel : SQLAsync.SQLStoredProcedureModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class MyDataModel : SQLAsync.SQLDataModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public async Task ExampleUsage(IConfiguration configuration)
{
    var sqlService = new SQLAsync.SQLServiceAsync(configuration);

    // Setting stored procedure name
    sqlService.StoredProcedureName = "MyStoredProcedure";

    // Adding parameters
    sqlService.AddParameter("@Id", SqlDbType.Int, 1);
    sqlService.AddParameter("@Name", "Example");

    // Executing a non-query asynchronously
    bool isAdded = await sqlService.AddAsync();

    // Selecting data asynchronously
    DataTable result = await sqlService.SelectAsync();
    List<Dictionary<string, object>> resultList = await sqlService.SelectListAsync();
    MyDataModel model = await sqlService.SelectModelAsync<MyDataModel>();

    // Inserting data asynchronously
    var newModel = new MyDataModel { Id = 2, Name = "New Example" };
    bool isInserted = await sqlService.InsertAsync("MyTable", newModel);

    // Updating data asynchronously
    var updateModel = new MyDataModel { Id = 2, Name = "Updated Example" };
    bool isUpdated = await sqlService.UpdateAsync("MyTable", "2", updateModel);

    // Deleting data asynchronously
    bool isDeleted = await sqlService.DeleteAsync("MyTable", "2");
}
```
