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

  ## Function
    ```C#
    // StoredProcedureName For Execute
    public string StoredProcedureName { get; set; }
    // set Parameter to Execute StoredProcedure
    public void AddParameter(string Name, SqlDbType DbType, object Data);
    public void AddParameter(string Name, object Data);
    public void AddParameterS<T>(T TModel) where T : SQLStoredProcedureModel;
    // if type of StoredProcedure is inserted must use this Function.this function return true or false
    public bool Add();
    // if type of StoredProcedure is Selected must use this Function.this function return DataTable
    public DataTable Select();
    // if type of StoredProcedure is Selected must use this Function.this function return List<Dictionary<string, object>> like Json
    public List<Dictionary<string, object>> SelectList();
    // if type of StoredProcedure is Selected must use this Function.this function return T. T is Object
    public T SelectModel<T>() where T : SQLDataModel;
    // if type of StoredProcedure is lot of table must use this Function.this function return DataSet
    public DataSet MultiSelect();
    // if type of StoredProcedure is lot of table must use this Function.this function return List<List<Dictionary<string, object>>> like Json
    public List<List<Dictionary<string, object>>> MultiSelectList();
    // if type of StoredProcedure is lot of table must use this Function.this function return T. T is Object
    public T MultiSelectModel<T>(string[] tableName) where T : SQLDataModel;
    //if you have a model like Table can insert value in model to table
    public bool Insert<T>(string tableName, T Model) where T : SQLDataModel;
    //if you have a model like Table can Update value in model to table
    public bool Update<T>(string tableName, string ID, T Model) where T : SQLDataModel;
    //if you have a ID in Table. can Delete row
    public bool Delete(string tableName, string ID);
 ```
