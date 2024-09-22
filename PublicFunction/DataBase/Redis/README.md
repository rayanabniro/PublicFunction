# Redis

  **Redis** is a source-available, in-memory storage, used as a distributed, in-memory key–value database, cache and message broker, with optional durability.


## PublicFunction.Database

  [README.md](https://github.com/rayanabniro/PublicFunction/blob/main/PublicFunction/DataBase/README.md "README.md")

# PublicFunction.Database.Redis

  In this **class**, we have infrastructures that can **centrally control** the connection to the **Redis**

## Use in .netCore
- in **appsettings.json** define Connection string like this

  ```json
      {
        "PublicFunction": {
          "DataBase": {
            "Redis": {
              "RedisManager": {
                "ConnectionString": "Redis Connection String"
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
    builder.Services.AddScoped<PublicFunction.Framework.DataBase.Redis.IRedisManager, PublicFunction.Framework.DataBase.Redis.RedisManager>();
  ```
- Suppose you want to use functions in a class, just write the following code in its constructor function
  ```C#
    public class Test
    {
        private readonly PublicFunction.Framework.DataBase.Redis.IRedisManager RedisManager;
        public BeforeLoginService(PublicFunction.Framework.DataBase.Redis.IRedisManager redismanager)
        {
            RedisManager = redismanager;
        }
        public TestAddKeyToredis()
        {
          string Key="Hi";
          string value="Hi Hi";
          TimeSpan Experation=DateTime.Now.TimeOfDay
          if(!RedisManager.EXISTS(Key))
            RedisManager.Set(Key,value,Experation);
        }
    }
  ```
## Function
```C#
  //redis Connection
  public ConnectionMultiplexer Connection { get; }
  //return IsConnecting Status
  public bool IsConnecting { get; }
  //return IsConnected Status
  public bool IsConnected { get; }
  //Get value Of Key
  public T Get<T>(string key);
  //Set Value from key and set experation TimeSpan in redis
  public void Set(string key, object value, TimeSpan experation);
  //Cheak Key EXISTS in redis
  public bool EXISTS(string key);
  //Delete Kay From redis
  public bool Delete(string key);
