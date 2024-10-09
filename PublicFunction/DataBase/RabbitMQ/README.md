
# RabbitMQService Class Documentation

## Interface: `IRabbitMQService`

This interface defines the basic contract for interacting with RabbitMQ queues, exchanges, and messages. It includes the following methods:

### Methods:
- `void Publish(string queueName, string message);`
    - **Description:** Sends a message to the specified queue.

- `void PublishWithExchange(string exchangeName, string routingKey, string message);`
    - **Description:** Sends a message to the specified exchange using the provided routing key.

- `void CreateQueue(string queueName, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null);`
    - **Description:** Creates a new queue with the specified options (durable, exclusive, auto-delete, etc.).

- `QueueDeclareOk GetQueueInfo(string queueName);`
    - **Description:** Retrieves information about the specified queue, such as the number of messages and its status.

- `void DeleteQueue(string queueName);`
    - **Description:** Deletes the specified queue.

- `void PurgeQueue(string queueName);`
    - **Description:** Removes all messages from the specified queue.

- `void Dispose();`
    - **Description:** Closes the RabbitMQ channel and connection, freeing resources.

---

## Class: `RabbitMQService`

This class provides the implementation of the `IRabbitMQService` interface and manages interactions with RabbitMQ using a connection, channel, and methods for queue and message handling.

### Constructor:

```csharp
public RabbitMQService(IConfiguration configuration)
```

- **Description:** Initializes the RabbitMQ service by reading configurations and setting up the connection and channel.

### Properties:

- `IConfiguration Configuration`: Stores the application configuration for connecting to RabbitMQ.
- `IConnection _connection`: Represents the RabbitMQ connection.
- `IModel _channel`: Represents the channel for interacting with RabbitMQ.

### Methods:

- `void Publish(string queueName, string message)`
    - **Description:** Publishes a message to a specific queue using the provided queue name.
    - **Example Usage:**
        ```csharp
        _rabbitMQService.Publish("logQueue", "This is a test message");
        ```

- `void PublishWithExchange(string exchangeName, string routingKey, string message)`
    - **Description:** Publishes a message to a specific exchange with the given routing key.
    - **Example Usage:**
        ```csharp
        _rabbitMQService.PublishWithExchange("logsExchange", "info", "This is a test message");
        ```

- `void CreateQueue(string queueName, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)`
    - **Description:** Creates a new queue with the specified options.
    - **Example Usage:**
        ```csharp
        _rabbitMQService.CreateQueue("taskQueue", true, false, false, null);
        ```

- `QueueDeclareOk GetQueueInfo(string queueName)`
    - **Description:** Retrieves passive information about the specified queue without modifying it.
    - **Example Usage:**
        ```csharp
        var queueInfo = _rabbitMQService.GetQueueInfo("logQueue");
        ```

- `void DeleteQueue(string queueName)`
    - **Description:** Deletes the specified queue.
    - **Example Usage:**
        ```csharp
        _rabbitMQService.DeleteQueue("oldQueue");
        ```

- `void PurgeQueue(string queueName)`
    - **Description:** Clears all messages from the specified queue.
    - **Example Usage:**
        ```csharp
        _rabbitMQService.PurgeQueue("logQueue");
        ```

- `void Dispose()`
    - **Description:** Closes the channel and the connection to RabbitMQ.
    - **Example Usage:**
        ```csharp
        _rabbitMQService.Dispose();
        ```

### Example of Use:

Here is an example of how to inject and use `RabbitMQService` in an ASP.NET Core project:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IRabbitMQService, RabbitMQService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Middleware and configurations
    }
}
```

This class and interface provide a robust way to interact with RabbitMQ within your .NET Core applications.

## appsettings.json
```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "VirtualHost": "/",
    "UserName": "guest",
    "Password": "guest",
    "Port": "5672"
  }
}
```
