
# RedisManager Class Documentation

## Overview
The `RedisManager` class provides an abstraction layer for interacting with a Redis database using the StackExchange.Redis library. It supports basic key-value operations, as well as more advanced operations on Redis data structures such as hashes, lists, and sets.

## Methods

### 1. `T Get<T>(string key)`
- **Description**: Retrieves a value from Redis based on the given key.
- **Parameters**: 
  - `key`: The Redis key to look up.
- **Returns**: The value associated with the key, deserialized to the specified type `T`.

### 2. `void Set(string key, object value, TimeSpan expiration)`
- **Description**: Stores a value in Redis with the specified key and expiration time.
- **Parameters**: 
  - `key`: The key under which the value will be stored.
  - `value`: The value to store.
  - `expiration`: The time span after which the key will expire.

### 3. `bool Exists(string key)`
- **Description**: Checks if a key exists in the Redis database.
- **Parameters**: 
  - `key`: The key to check.
- **Returns**: `true` if the key exists, `false` otherwise.

### 4. `bool Delete(string key)`
- **Description**: Deletes a key from Redis.
- **Parameters**: 
  - `key`: The key to delete.
- **Returns**: `true` if the key was deleted, `false` if it did not exist.

### 5. `Task<bool> HashExistsAsync(string hashKey, string field)`
- **Description**: Checks if a field exists in a Redis hash.
- **Parameters**: 
  - `hashKey`: The Redis hash key.
  - `field`: The field to check within the hash.
- **Returns**: A task representing the result of the asynchronous operation.

### 6. `Task<T> HashGetAsync<T>(string hashKey, string field)`
- **Description**: Retrieves the value of a specific field from a Redis hash.
- **Parameters**: 
  - `hashKey`: The Redis hash key.
  - `field`: The field to retrieve.
- **Returns**: The value of the field, deserialized to the specified type `T`.

### 7. `Task HashSetAsync(string hashKey, string field, object value)`
- **Description**: Sets the value of a specific field in a Redis hash.
- **Parameters**: 
  - `hashKey`: The Redis hash key.
  - `field`: The field to set.
  - `value`: The value to store in the field.

### 8. `Task<long> ListRightPushAsync(string listKey, string value)`
- **Description**: Adds an item to the end of a Redis list.
- **Parameters**: 
  - `listKey`: The key of the list.
  - `value`: The value to add to the list.
- **Returns**: The length of the list after the push operation.

### 9. `Task<string> ListLeftPopAsync(string listKey)`
- **Description**: Removes and returns the first item from a Redis list.
- **Parameters**: 
  - `listKey`: The key of the list.
- **Returns**: The value of the first item removed from the list.

### 10. `Task<long> SetAddAsync(string setKey, string value)`
- **Description**: Adds an item to a Redis set.
- **Parameters**: 
  - `setKey`: The key of the set.
  - `value`: The value to add to the set.
- **Returns**: The number of elements added to the set.

### 11. `Task<bool> SetContainsAsync(string setKey, string value)`
- **Description**: Checks if a Redis set contains a specific value.
- **Parameters**: 
  - `setKey`: The key of the set.
  - `value`: The value to check for.
- **Returns**: `true` if the set contains the value, `false` otherwise.

### 12. `Task<RedisValue[]> SetMembersAsync(string setKey)`
- **Description**: Retrieves all members of a Redis set.
- **Parameters**: 
  - `setKey`: The key of the set.
- **Returns**: An array of `RedisValue` representing the members of the set.

### 13. `void Dispose()`
- **Description**: Closes the connection to the Redis server and releases resources.

## Usage Example

```csharp
var redisManager = new RedisManager(configuration);

// Store a value
redisManager.Set("myKey", "myValue", TimeSpan.FromMinutes(5));

// Retrieve a value
var value = redisManager.Get<string>("myKey");

// Check if a key exists
bool exists = redisManager.Exists("myKey");

// Delete a key
bool deleted = redisManager.Delete("myKey");

// Work with hashes
await redisManager.HashSetAsync("myHash", "myField", "myHashValue");
var hashValue = await redisManager.HashGetAsync<string>("myHash", "myField");

// Work with lists
await redisManager.ListRightPushAsync("myList", "myListValue");
var firstItem = await redisManager.ListLeftPopAsync("myList");

// Work with sets
await redisManager.SetAddAsync("mySet", "mySetValue");
bool isMember = await redisManager.SetContainsAsync("mySet", "mySetValue");
var members = await redisManager.SetMembersAsync("mySet");
```

## Notes
- Ensure that the Redis connection string is correctly configured in your application's configuration settings.
- Use `Dispose` to close the Redis connection when it is no longer needed.
