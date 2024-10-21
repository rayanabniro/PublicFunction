# PublicFunction.Converter

In this **class**, we have infrastructures that can **centrally control** For Converter 

## Use in .netCore
- in **Program.cs**  Add This dependence
  ```C#
    builder.Services.AddScoped<PublicFunction.Converter.IJson, PublicFunction.Converter.Json>();
  ```
- Suppose you want to use functions in a class, just write the following code in its constructor function
  ```C#
    public class Test
    	{
    		private readonly PublicFunction.Converter.IJson Json;
    		public BeforeLoginService(PublicFunction.Converter.IJson json)
    		{
    		    Json = json;
    		}
    	}
  ```

  ## Function
    ```C#
        // Json string To Object or class T
        public T Deserialize<T>(string json);
        // T obj to string
        public string Serialize<T>(T obj);
    ```
