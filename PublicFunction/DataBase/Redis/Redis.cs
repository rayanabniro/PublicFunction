using Microsoft.Extensions.Configuration;
using StackExchange.Redis;


namespace PublicFunction.DataBase;
public class Redis
{
    public interface IRedisManager
    {
        public ConnectionMultiplexer Connection { get; }
        public bool IsConnecting { get; }

        public bool IsConnected { get; }
        public T Get<T>(string key);
        public void Set(string key, object value, TimeSpan experation);
        public bool EXISTS(string key);
        public bool Delete(string key);
    }
    public class RedisManager : IRedisManager
    {
        private readonly IConfiguration Configuration;
        private string ConnectionString;
        Lazy<ConnectionMultiplexer> redis = new Lazy<ConnectionMultiplexer>();
        public RedisManager(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration["PublicFunction:DataBase:Redis:RedisManager:ConnectionString"].ToString();
            redis = new Lazy<ConnectionMultiplexer>(delegate
            {
                try
                {
                    return ConnectionMultiplexer.Connect(new ConfigurationOptions
                    {
                        AbortOnConnectFail = false,
                        EndPoints = { ConnectionString }
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

        }



        public ConnectionMultiplexer Connection => redis.Value;

        public bool IsConnecting => redis.Value.IsConnecting;

        public bool IsConnected => redis.Value.IsConnected;

        public T Get<T>(string key)
        {
            try
            {
                if (redis == null)
                {
                    return default;
                }

                RedisValue redisValue = redis.Value.GetDatabase().StringGet(key);
                if (!redisValue.IsNull)
                {
                    return new Converter.Json().Deserialize<T>(redisValue);
                }

                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Set(string key, object value, TimeSpan experation)
        {
            if (redis != null)
            {
                redis.Value.GetDatabase().StringSet(key, new Converter.Json().Serialize(value), experation);
            }
        }
        public bool EXISTS(string key)
        {
            try
            {
                if (redis == null)
                {
                    return false;
                }

                return redis.Value.GetDatabase().KeyExists(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete(string key)
        {
            try
            {
                if (redis == null)
                {
                    return false;
                }

                return redis.Value.GetDatabase().KeyDelete(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}