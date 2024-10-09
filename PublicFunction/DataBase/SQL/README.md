# Microsoft SQL Server

**Microsoft SQL Server** is a proprietary relational database management system developed by Microsoft. As a database server, it is a software product with the primary function of storing and retrieving


## PublicFunction.Database

[README.md](https://github.com/rayanabniro/PublicFunction/blob/main/PublicFunction/DataBase/README.md "README.md")

# PublicFunction.Database.SQL

In this **class**, we have infrastructures that can **centrally control** the connection to the **sql database**

## Use in .netCore
- in **appsettings.json** define Connection string like this
    ```json
        {
          "PublicFunction": {
            "DataBase": {
              "SQL": {
                "SQLService": {
                  "ConnectionString": "sql Connection String",
                  "ConnectionStringMirror":"for All Select Class Use this Connection String"
                }
              }
            }
          }
        }
    ```
- in **Program.cs**  Add This dependence
  ```C#
    builder.Services.AddSingleton<IConfiguration>(_ => configuration);
  ```
- in **Program.cs**  Add This dependence
  ```C#
    builder.Services.AddScoped<PublicFunction.Framework.DataBase.SQL.ISQLService, PublicFunction.Framework.DataBase.SQL.SQLService>();
  ```
- Suppose you want to use functions in a class, just write the following code in its constructor function
  ```C#
    public class Test
    	{
    		private readonly PublicFunction.Framework.DataBase.SQL.ISQLService SQLService;
    		public BeforeLoginService(PublicFunction.Framework.DataBase.SQL.ISQLService sqlservice)
    		{
    		    SQLService = sqlservice;
    		}
    	}
  ```

  # SQLService Documentation

## Overview
`SQLService` is a class that provides methods to interact with a SQL database using stored procedures and direct SQL commands. It implements the `ISQLService` interface.

## Classes and Interfaces

### SQLStoredProcedureModel
A base class for models used in stored procedures.

### SQLDataModel
A base class for data models.

### ISQLService Interface
Defines the methods and properties for SQLService.

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

- `bool Add()`
  - Executes a non-query SQL command.

- `DataTable Select()`
  - Executes a query and returns the result as a DataTable.

- `List<Dictionary<string, object>> SelectList()`
  - Executes a query and returns the result as a list of dictionaries.

- `T SelectModel<T>() where T : SQLDataModel`
  - Executes a query and maps the result to a model of type `T`.

- `DataSet MultiSelect()`
  - Executes a query and returns the result as a DataSet.

- `List<List<Dictionary<string, object>>> MultiSelectList()`
  - Executes a query and returns the result as a list of lists of dictionaries.

- `T MultiSelectModel<T>(string[] tableNames) where T : SQLDataModel`
  - Executes a query and maps the result to a model of type `T` based on the specified table names.

- `bool Insert<T>(string tableName, T Model) where T : SQLDataModel`
  - Inserts a new record into the specified table using the provided model.

- `bool Update<T>(string tableName, string ID, T Model) where T : SQLDataModel`
  - Updates an existing record in the specified table using the provided model and ID.

- `bool Delete(string tableName, string ID)`
  - Deletes a record from the specified table using the provided ID.

## Example Usage

```csharp
public class MyStoredProcedureModel : SQL.SQLStoredProcedureModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class MyDataModel : SQL.SQLDataModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public void ExampleUsage(IConfiguration configuration)
{
    var sqlService = new SQL.SQLService(configuration);

    // Setting stored procedure name
    sqlService.StoredProcedureName = "MyStoredProcedure";

    // Adding parameters
    sqlService.AddParameter("@Id", SqlDbType.Int, 1);
    sqlService.AddParameter("@Name", "Example");

    // Executing a non-query
    bool isAdded = sqlService.Add();

    // Selecting data
    DataTable result = sqlService.Select();
    List<Dictionary<string, object>> resultList = sqlService.SelectList();
    MyDataModel model = sqlService.SelectModel<MyDataModel>();

    // Inserting data
    var newModel = new MyDataModel { Id = 2, Name = "New Example" };
    bool isInserted = sqlService.Insert("MyTable", newModel);

    // Updating data
    var updateModel = new MyDataModel { Id = 2, Name = "Updated Example" };
    bool isUpdated = sqlService.Update("MyTable", "2", updateModel);

    // Deleting data
    bool isDeleted = sqlService.Delete("MyTable", "2");
}