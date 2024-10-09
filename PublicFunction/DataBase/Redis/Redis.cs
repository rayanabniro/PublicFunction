using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PublicFunction.DataBase;
public class Redis
{
    public interface IRedisManager
    {
        ConnectionMultiplexer Connection { get; }
        bool IsConnecting { get; }
        bool IsConnected { get; }

        T Get<T>(string key);
        void Set(string key, object value, TimeSpan expiration);
        bool Exists(string key);
        bool Delete(string key);

        // Additional Redis operations
        Task<bool> HashExistsAsync(string hashKey, string field);
        Task<T> HashGetAsync<T>(string hashKey, string field);
        Task HashSetAsync(string hashKey, string field, object value);
        Task<long> ListRightPushAsync(string listKey, string value);
        Task<string> ListLeftPopAsync(string listKey);
        Task<bool> SetAddAsync(string setKey, string value);
        Task<bool> SetContainsAsync(string setKey, string value);
        Task<RedisValue[]> SetMembersAsync(string setKey);
    }

    public class RedisManager : IRedisManager, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly Lazy<ConnectionMultiplexer> _redis;

        public RedisManager(IConfiguration configuration)
        {
            _configuration = configuration;
            var connectionString = _configuration["PublicFunction:DataBase:Redis:RedisManager:ConnectionString"];

            _redis = new Lazy<ConnectionMultiplexer>(() =>
            {
                try
                {
                    return ConnectionMultiplexer.Connect(new ConfigurationOptions
                    {
                        AbortOnConnectFail = false,
                        EndPoints = { connectionString }
                    });
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Could not connect to Redis", ex);
                }
            });
        }

        public ConnectionMultiplexer Connection => _redis.Value;
        public bool IsConnecting => _redis.Value.IsConnecting;
        public bool IsConnected => _redis.Value.IsConnected;

        public T Get<T>(string key)
        {
            var redisValue = _redis.Value.GetDatabase().StringGet(key);
            return !redisValue.IsNull ? new Converter.Json().Deserialize<T>(redisValue) : default;
        }

        public void Set(string key, object value, TimeSpan expiration)
        {
            _redis.Value.GetDatabase().StringSet(key, new Converter.Json().Serialize(value), expiration);
        }

        public bool Exists(string key)
        {
            return _redis.Value.GetDatabase().KeyExists(key);
        }

        public bool Delete(string key)
        {
            return _redis.Value.GetDatabase().KeyDelete(key);
        }

        // Additional Redis Hash Operations
        public async Task<bool> HashExistsAsync(string hashKey, string field)
        {
            return await _redis.Value.GetDatabase().HashExistsAsync(hashKey, field);
        }

        public async Task<T> HashGetAsync<T>(string hashKey, string field)
        {
            var redisValue = await _redis.Value.GetDatabase().HashGetAsync(hashKey, field);
            return !redisValue.IsNull ? new Converter.Json().Deserialize<T>(redisValue) : default;
        }

        public async Task HashSetAsync(string hashKey, string field, object value)
        {
            await _redis.Value.GetDatabase().HashSetAsync(hashKey, field, new Converter.Json().Serialize(value));
        }

        // Additional Redis List Operations
        public async Task<long> ListRightPushAsync(string listKey, string value)
        {
            return await _redis.Value.GetDatabase().ListRightPushAsync(listKey, value);
        }

        public async Task<string> ListLeftPopAsync(string listKey)
        {
            return await _redis.Value.GetDatabase().ListLeftPopAsync(listKey);
        }

        // Additional Redis Set Operations
        public async Task<bool> SetAddAsync(string setKey, string value)
        {
            return await _redis.Value.GetDatabase().SetAddAsync(setKey, value);
        }

        public async Task<bool> SetContainsAsync(string setKey, string value)
        {
            return await _redis.Value.GetDatabase().SetContainsAsync(setKey, value);
        }

        public async Task<RedisValue[]> SetMembersAsync(string setKey)
        {
            return await _redis.Value.GetDatabase().SetMembersAsync(setKey);
        }

        public void Dispose()
        {
            if (_redis.IsValueCreated)
            {
                _redis.Value.Dispose();
            }
        }
    }
}