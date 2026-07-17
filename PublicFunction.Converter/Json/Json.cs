using System;
using System.Text.Json;

namespace PublicFunction.Converter
{
    public class Json
    {
        public interface IJsonService
        {
           public T Deserialize<T>(string json);
           public string Serialize<T>(T obj, JsonSerializerOptions? options = null);
        }

        public class JsonService : IJsonService
        {
            public T Deserialize<T>(string json)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(json);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error deserializing JSON.", ex);
                }
            }

            public string Serialize<T>(T obj, JsonSerializerOptions? options = null)
            {
                try
                {
                    options ??= new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    return JsonSerializer.Serialize(obj, options);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error serializing object to JSON.", ex);
                }
            }
        }
    }
}
