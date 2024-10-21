using System;
using System.Text.Json;

namespace PublicFunction.Converter
{
    public class Json
    {
        public T Deserialize<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Serialize<T>(T obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}