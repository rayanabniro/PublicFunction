using System;
using System.Text.Json;

namespace PublicFunction.Converter
{
    interface IJson
    {
        public T Deserialize<T>(string json);
        public string Serialize<T>(T obj);
    }

    public class Json : IJson
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

        public string Serialize<T>(T obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error serializing object to JSON.", ex);
            }
        }
    }
}
