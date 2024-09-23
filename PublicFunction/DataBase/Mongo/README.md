# MongoDB

**MongoDB** is a source-available, cross-platform, document-oriented database program. Classified as a **NoSQL** database product, MongoDB utilizes **JSON-like** documents with optional schemas. MongoDB is developed by MongoDB Inc. and current versions are licensed under the Server Side Public License


## PublicFunction.Database

[README.md](https://github.com/rayanabniro/PublicFunction/blob/main/PublicFunction/DataBase/README.md "README.md")

# PublicFunction.Database.Mongo

In this **class**, we have infrastructures that can **centrally control** the connection to the **Mongo database**

## Use in .netCore
- in **appsettings.json** define Connection string like this
    ```json
        {
          "PublicFunction": {
            "DataBase": {
              "Mongo": {
                "ConnectionString": "Mongo Connection String",
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
    builder.Services.AddScoped<PublicFunction.Framework.DataBase.Mongo.IMongo, PublicFunction.Framework.DataBase.Mongo.Mongo>();
  ```
- Suppose you want to use functions in a class, just write the following code in its constructor function
  ```C#
    public class Test
    	{
    		private readonly PublicFunction.Framework.DataBase.Mongo.IMongo MongoService;
    		public BeforeLoginService(PublicFunction.Framework.DataBase.Mongo.IMongo mongoService)
    		{
    		    MongoService = mongoService;
    		}
    	}
  ```

  ## Function
    ```C#
        //insert T Model to Mongo Collection
        public bool Insert<T>(string CollectionName, T BaseCollection) where T : TBaseDocumentModel;
        //insert List<Dictionary<string, object>> to Mongo Collection
        public bool Insert(string CollectionName, List<Dictionary<string, object>> BaseCollection);
        //insert Dictionary<string, object> to Mongo Collection
        public bool Insert(string CollectionName, Dictionary<string, object> BaseCollection);
   ```

