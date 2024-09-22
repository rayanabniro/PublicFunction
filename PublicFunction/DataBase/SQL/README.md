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
