# PublicFunction.Middleware.AuthMiddleware

1. Authentication:

Authentication refers to verifying the identity of a user. This process checks whether a user is who they claim to be. The most common methods for authentication are:

    OAuth 2.0: A standard protocol for securely granting access to resources using tokens. This protocol allows applications to manage user access to different resources without needing to store login credentials.
    JWT (JSON Web Token): A method for securely transmitting data. JWT is often used for transmitting authentication information, including user ID and token expiration time.

2. Authorization:

Authorization refers to determining what a user is allowed to do. This process takes place after authentication and checks whether the user is permitted to access specific resources. Common approaches to managing authorization include:

    Role-Based Access Control (RBAC): Assigns permissions based on user roles. For example, users with an "admin" role may have access to certain resources that are not available to "employee" users.
    Attribute-Based Access Control (ABAC): Assigns permissions based on user attributes such as age, geographical location, etc.

Using OAuth and JWT for Authentication & Authorization:

    OAuth:
        The user first logs in and requests authorization to access specific resources.
        The system requests the identity provider (IDP) to authenticate the user.
        After successful authentication, the server issues an access token.
        This token is used to access protected resources.

    JWT:
        After successful login, the server generates a JWT token containing encrypted user information.
        This token is sent in every request to the server, where the server verifies the token for authentication and authorization.
        JWT is fast and suitable for scalable systems due to its Base64 URL-encoded structure.

Key Considerations:

    Security: Tokens should be transmitted securely (e.g., via HTTPS) and should not be stored in the browser or other insecure locations.
    Expiration: For additional security, tokens typically have an expiration date, and users must log in again or refresh their tokens after they expire.
    Access Restriction: Using scope and permissions in OAuth and features within JWT, access can be finely controlled.


    Configuration in Startup.cs:

Here’s how you would configure the middleware in the Configure method of Startup.cs:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Add authentication services (JWT)
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidAudience = Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Use the auth middleware
        app.UseAuthMiddleware(
            Configuration["JwtSettings:SecretKey"],
            Configuration["JwtSettings:Issuer"],
            Configuration["JwtSettings:Audience"]
        );

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

```

Explanation of the Middleware:

    Authorization Header: The middleware checks if the Authorization header contains a valid Bearer token.
    JWT Validation: It validates the token using the TokenValidationParameters. This includes checking the issuer, audience, signing key, and token lifetime.
    User Roles: After successful validation, it checks whether the user has a specific role (e.g., "admin") before proceeding.
    Error Handling: If there’s an issue with the token or authorization, an appropriate HTTP status code (401 Unauthorized or 403 Forbidden) is returned.

Configuration:

    The JwtSettings (like SecretKey, Issuer, and Audience) are retrieved from the configuration, allowing the developer to set them based on their environment or application settings.

This example provides a robust and customizable way to manage authentication and authorization in .NET Core applications.


# PublicFunction.Middleware.FirewallAntiDdosMiddleware

### Firewall & Anti-DDoS Middleware Explanation:

**Firewall** and **Anti-DDoS (Distributed Denial of Service)** protection are essential components in securing web applications from malicious attacks and excessive traffic that could degrade or disrupt the service. These measures are designed to filter out harmful requests, block traffic from known bad sources, and limit traffic that exceeds acceptable thresholds.

-   **Firewall**: Acts as a filter between trusted internal networks and untrusted external networks, usually blocking access to certain IPs, ports, or services. It inspects incoming requests and applies rules that define which traffic is allowed and which is blocked.
    
-   **Anti-DDoS**: Refers to techniques used to mitigate DDoS attacks, where an attacker floods a service with an overwhelming amount of requests, aiming to exhaust server resources and cause downtime. Anti-DDoS middleware typically limits the rate of requests from the same IP or restricts excessive connections in a short time period.
    

### Features of Firewall & Anti-DDoS Middleware:

1.  **IP Whitelisting/Blacklisting**: Allows specific trusted IPs to access your application while blocking malicious or unwanted IPs.
2.  **Rate Limiting**: Limits the number of requests a client can make in a given time window (e.g., 100 requests per minute).
3.  **Request Filtering**: Filters out requests that contain malicious payloads or are formatted incorrectly.
4.  **Traffic Monitoring**: Monitors traffic patterns and detects sudden spikes or patterns that might indicate a DDoS attack.
5.  **Geo-blocking**: Blocks or limits traffic based on the geographic location of the IP address.

### Middleware Code for Firewall & Anti-DDoS in .NET Core

Here’s how you can implement a **Firewall & Anti-DDoS** middleware in .NET Core. This middleware will allow you to:

-   Block specific IPs.
-   Rate-limit requests from the same IP address.
-   Track traffic patterns to mitigate DDoS attacks.


### How It Works:

1.  **Blocked IPs**: The middleware checks whether the incoming request comes from a blacklisted IP. If the IP is blocked, a `403 Forbidden` response is sent.
    
2.  **Rate Limiting**:
    
    -   It tracks the number of requests each IP has made within a certain time window (1 minute in this example).
    -   If an IP exceeds the defined limit (e.g., 100 requests per minute), the middleware returns a `429 Too Many Requests` response.
    -   The request count is reset after the time window (1 minute) expires.
3.  **Logging**: Each time a request is blocked or rate-limited, a log entry is created for monitoring and debugging purposes.
    

### Configuration in `Startup.cs`:

You need to configure the middleware in the `Configure` method of `Startup.cs`:

```csharp
public class Startup
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Use the firewall and anti-DDoS middleware
        app.UseFirewallAntiDdos();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

```

### Explanation of the Middleware:

-   **Blocked IPs**: In the `IsBlacklisted` method, you can maintain an array or list of blocked IPs. If a request is made from one of these IPs, it is denied access.
-   **Rate Limiting**: In the `RateLimitRequest` method, the middleware increments a request count for each IP. If the number of requests exceeds the limit, it blocks further requests for that time window.
-   **Logging**: Every time a request is blocked due to being blacklisted or exceeding the rate limit, a log message is generated.

### Additional Features (optional):

-   **Geolocation-based Blocking**: Use a service like [IPstack](https://ipstack.com/) to determine the geographical location of IP addresses and block traffic from specific countries or regions.
-   **Dynamic Blacklist**: You can implement a more sophisticated system where IPs are dynamically added to the blacklist based on abnormal traffic patterns.

### Testing:

-   **Blacklisted IP Test**: Send a request from an IP in the blacklist, and you should receive a `403 Forbidden` response.
-   **Rate Limit Test**: Send more than the allowed number of requests from the same IP within the rate-limiting window, and you will receive a `429 Too Many Requests` response.

This middleware is an efficient way to protect your application from both malicious traffic (via blacklisting) and overloads (via rate-limiting).



# PublicFunction.Middleware.RateLimitingMiddleware
**Rate Limiting** is a technique used to control the number of requests a user can make to an API or web service within a specified time period. It helps to prevent abuse, avoid overloading the system, and ensures fair use of resources. Rate limiting can be applied to protect services from DoS (Denial of Service) attacks, prevent resource exhaustion, and manage usage quotas.

For example, a service may allow only 100 requests per minute per IP address to prevent overuse of resources.

### Benefits of Rate Limiting:

1.  **Prevent abuse**: Protect the application from misuse or intentional abuse.
2.  **Ensure fair usage**: Limit the number of requests per user or IP to ensure that all users have fair access to resources.
3.  **Improve performance**: Prevent the server from being overwhelmed by too many requests in a short period.
4.  **Protection against DDoS**: Reduce the impact of Distributed Denial of Service (DDoS) attacks.

### Rate Limiting Strategies:

-   **Fixed Window**: Allows a fixed number of requests in a fixed time window (e.g., 100 requests per minute).
-   **Sliding Window**: Allows a fixed number of requests, but the window shifts over time.
-   **Token Bucket**: Requests are allowed as long as tokens are available in a bucket. Tokens are replenished at a fixed rate.
-   **Leaky Bucket**: Similar to token bucket, but the request rate is smoothed over time.

### Rate Limiting Middleware Implementation in .NET Core

In this implementation, we’ll create a **Fixed Window Rate Limiting** middleware that limits the number of requests per user (identified by IP address) within a 1-minute window.

Here’s the step-by-step guide to implement a simple rate-limiting middleware.

### Explanation:

1.  **Request Count**: We maintain a count of requests for each IP address in memory using `IMemoryCache`. The count is incremented each time a request is received from that IP.
2.  **Cache Key**: The cache key is generated based on the client's IP address to keep track of individual IPs.
3.  **Request Limit**: We set a limit (e.g., 100 requests) for a 1-minute window. If a user exceeds this limit, they get a `429 Too Many Requests` HTTP status code.
4.  **Sliding Time Window**: Each IP's request count is reset after the specified time window (in this case, 1 minute). This means that after 1 minute, the request count starts over.

### How to Use:

#### Step 1: Register the Middleware in `Startup.cs`

To use the rate-limiting middleware, you need to register it in the `Configure` method of your `Startup.cs`.

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Use rate limiting middleware
    app.UseRateLimiting();

    app.UseRouting();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

```
#### Step 2: Configure Memory Cache in `Startup.cs`

You will also need to configure the `IMemoryCache` service, which is required for storing the request count. Add the following to the `ConfigureServices` method in `Startup.cs`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMemoryCache(); // Adds in-memory cache
    services.AddControllers();
}

```

### Example Behavior:

#### Scenario 1: Within Rate Limit

-   **Request**: User makes 50 requests within a minute.
-   **Response**: The user continues to receive normal responses with a `200 OK` status.

#### Scenario 2: Rate Limit Exceeded

-   **Request**: User makes 101 requests within the same minute.
-   **Response**: The user will receive a `429 Too Many Requests` response with a message like "Rate limit exceeded. Try again later."

### Handling Edge Cases:

1.  **Cache Expiry**: If the cache expires (after 1 minute in our case), the user's request count is reset.
2.  **Different Clients**: Rate limiting is applied per IP address in this example, so different clients (e.g., different IP addresses) have independent limits.
3.  **Logging**: You can log the rate-limiting events using `ILogger` to keep track of users exceeding limits.

### Advanced Enhancements:

1.  **Dynamic Limits**: You could enhance this middleware to allow rate limits based on user roles, specific paths, or other criteria.
2.  **Distributed Rate Limiting**: For distributed systems with multiple instances of your application, consider using distributed caches like **Redis** to maintain the rate limit across all instances.

### Conclusion:

This middleware provides basic **Rate Limiting** using in-memory caching, allowing you to limit the number of requests a user can make within a specified time window. It's a useful approach for protecting APIs from abuse or overuse. You can adjust the time window and request limits as per your requirements, or extend it with more advanced strategies like distributed rate limiting if needed.

# PublicFunction.Middleware.LoadBalancingMiddleware

### Load Balancing Middleware Explanation

**Load Balancing** is a technique used to distribute incoming network traffic across multiple servers or resources to ensure no single server becomes overwhelmed. It enhances application reliability, improves performance, and ensures high availability by managing traffic efficiently.

In the context of a middleware implementation, **Load Balancing** typically involves redirecting incoming requests to different servers or services based on certain rules or strategies. These strategies can be **round-robin**, **least connections**, **IP hash**, or **random** among others.

### Benefits of Load Balancing:

1.  **Improved Scalability**: By distributing traffic across multiple servers, you can handle more concurrent requests.
2.  **Fault Tolerance**: If one server goes down, the traffic is redirected to other healthy servers, ensuring high availability.
3.  **Optimized Performance**: Load balancing helps in reducing latency and optimizing resource usage.

### Load Balancing Strategies:

1.  **Round-robin**: Distributes requests equally among servers in a cyclic manner.
2.  **Least connections**: Directs requests to the server with the fewest active connections.
3.  **IP Hash**: Uses the client's IP address to determine which server to route the request to.
4.  **Weighted Distribution**: Assigns weights to servers based on their capacity, and routes traffic accordingly.

### Load Balancing Middleware Implementation in .NET Core

In this example, we'll implement a **Round-robin load balancing middleware** that distributes incoming requests across multiple backend servers. The backend servers could be a list of IPs or URLs to which requests are routed.


### Explanation:

1.  **Backend Servers**:
    
    -   A list of backend servers (`_backendServers`) is maintained. These can be local IPs or URLs pointing to different instances of your service.
    -   In the example, three local servers (e.g., `localhost:5001`, `localhost:5002`, etc.) are used, but in production, these could be load balancers or service instances deployed in a cloud environment.
2.  **Round-Robin Strategy**:
    
    -   The middleware selects the next server in the list based on a round-robin strategy. Each time a request comes in, the server index (`_currentServerIndex`) is incremented, and it loops back when it reaches the end of the list.
3.  **Forwarding the Request**:
    
    -   The middleware uses `HttpClient` to forward the incoming request to the selected backend server.
    -   It copies the request method, headers, and body from the original request to the forwarded request.
    -   The backend server's response is then copied back to the original response, including the status code and headers.
4.  **Error Handling**:
    
    -   This basic implementation does not include error handling, but in a production environment, you might want to implement fallback mechanisms, such as retrying requests on another server if one fails.

### How to Use the Middleware:

#### Step 1: Register the Middleware in `Startup.cs`

To use the load balancing middleware, add it to the middleware pipeline in the `Configure` method of your `Startup.cs`:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
{ 
// Use the Load Balancing Middleware 
app.UseLoadBalancing(); 
app.UseRouting(); 
app.UseAuthorization(); 
app.UseEndpoints(endpoints => { endpoints.MapControllers(); }); 
}
```
#### Step 2: Configure Backend Servers

-   Modify the list of backend servers in the `LoadBalancingMiddleware` to include the actual addresses of your application servers.
-   In a production environment, these might be IP addresses or service endpoints behind a load balancer or containerized microservices.

### Example Behavior:

1.  **Request**: A client makes an HTTP request to your application.
2.  **Load Balancing**: The load balancing middleware selects the next server from the list using a round-robin approach.
3.  **Forwarding**: The middleware forwards the request to the chosen server.
4.  **Response**: The response from the backend server is forwarded back to the client.

### Advanced Enhancements:

1.  **Dynamic Server List**:
    
    -   Instead of hardcoding the list of backend servers, you could pull it from a configuration file, database, or service registry (e.g., Consul, Eureka).
2.  **Health Checks**:
    
    -   Implement health checks to ensure that only healthy servers are included in the list. This way, the load balancer can skip over servers that are down or experiencing issues.
3.  **Weighted Load Balancing**:
    
    -   Implement a weighted round-robin approach where certain servers handle more requests than others based on their capacity (e.g., a more powerful server can handle more traffic).
4.  **Failover and Retry Logic**:
    
    -   Add failover support. If a server fails or responds with an error, the middleware can retry the request on another server.

### Conclusion:

This Load Balancing Middleware example distributes requests across multiple backend servers using a round-robin strategy. It's a simple but effective way to scale applications and improve fault tolerance. For production systems, you may want to add more advanced features such as dynamic server lists, health checks, or retry mechanisms to improve resilience.



# PublicFunction.Middleware.ApiVersioningMiddleware

### API Versioning Middleware Explanation

**API Versioning** is essential for managing changes in your API over time. It ensures that new versions of the API can be introduced while maintaining backward compatibility for clients using older versions. Versioning helps when you need to make breaking changes or introduce new features without disrupting existing clients.

There are several common strategies for API versioning:

1.  **Path-based versioning** (e.g., `/api/v1/resource`, `/api/v2/resource`)
2.  **Query parameter versioning** (e.g., `/api/resource?version=1`)
3.  **Header versioning** (e.g., via `Accept` headers like `application/vnd.myapi.v1+json`)

In this example, we’ll implement **path-based API versioning**. This approach involves embedding the version number directly in the API's URL, making it clear which version of the API a client is accessing.

### Why Use API Versioning?

1.  **Backward Compatibility**: Ensures that existing clients don't break when new features are added or changes are made to the API.
2.  **Smooth Transitions**: Clients can transition to the latest version of the API at their own pace without disrupting existing services.
3.  **Flexibility**: Provides flexibility in deploying new API versions while maintaining old versions.

### Step-by-Step Guide to Implement API Versioning Middleware:

This middleware will:

1.  Extract the version from the URL path (e.g., `/api/v1/resource`).
2.  Pass the version to the controllers via `HttpContext`.
3.  Allow controllers to use the version information to handle version-specific logic.

### Explanation:

1.  **Extracting the Version**:
    
    -   The middleware looks for a version segment in the URL path. It assumes the version will be indicated as a "v" followed by a number (e.g., `/api/v1/resource` or `/api/v2/resource`).
    -   It extracts the version number and stores it in the `HttpContext.Items` collection, which can be accessed later by controllers or other middlewares.
2.  **Default Versioning**:
    
    -   If no version is specified in the request path, the middleware defaults to version 1 (`"1"`). This can be adjusted based on your needs.
3.  **Logging**:
    
    -   For debugging and tracking purposes, the version information is logged using `ILogger`.
4.  **Passing Version Info**:
    
    -   The API version is saved in `HttpContext.Items["ApiVersion"]`. This allows subsequent middleware or controllers to access the version information.

### How to Use the Middleware:

#### Step 1: Register the Middleware in `Startup.cs`

In the `Configure` method of `Startup.cs`, add the versioning middleware to the request pipeline:


```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Use API versioning middleware
    app.UseApiVersioning();

    app.UseRouting();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

```

#### Step 2: Create Version-Specific Controllers

You should create separate controllers for each version of your API. Here’s an example of versioned controllers:
```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class ProductsV1Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Message = "This is API version 1" });
    }
}

[ApiController]
[Route("api/v2/[controller]")]
public class ProductsV2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Message = "This is API version 2" });
    }
}

```

### Explanation of the Controllers:

-   **ProductsV1Controller**: Handles requests to `/api/v1/products`.
-   **ProductsV2Controller**: Handles requests to `/api/v2/products`.

You can extend this by having more versions (e.g., `v3`, `v4`, etc.) and create controllers that handle requests for those versions.

### Example Request and Response:

1.  **Request**: `GET /api/v1/products`
    
    -   **Response**: `{ "Message": "This is API version 1" }`
2.  **Request**: `GET /api/v2/products`
    
    -   **Response**: `{ "Message": "This is API version 2" }`
3.  **Request**: `GET /api/products` (without specifying a version)
    
    -   **Response**: `{ "Message": "This is API version 1" }` (defaults to version 1)

### Advanced Features and Customization:

1.  **Query Parameter Versioning**:
    
    -   You could enhance the middleware to also support query parameter versioning (e.g., `/api/products?version=1`). This is useful when you want clients to pass the version explicitly in the query string instead of the URL path.
2.  **Header Versioning**:
    
    -   You can modify the middleware to check the request headers for a version (e.g., via an `Accept` header or a custom header like `X-API-Version`). This is useful in situations where you want versioning to be transparent in the URL.
3.  **Version Negotiation**:
    
    -   You could implement logic that dynamically determines which version to serve based on the client's request headers or other criteria, rather than having fixed versioning paths.
4.  **Error Handling**:
    
    -   If an unsupported version is requested, you can return a custom error message (e.g., `404 Not Found` or `400 Bad Request`).

### Conclusion:

This **API Versioning Middleware** provides a straightforward way to manage multiple versions of your API using path-based versioning. It gives clients the ability to access specific versions of the API while keeping the API backward-compatible and flexible for future changes. This approach can be extended to handle query-based or header-based versioning as needed.




# PublicFunction.Middleware.AsynchronousProcessingMiddleware

### Asynchronous Processing Middleware Explanation

**Asynchronous Processing Middleware** is used to handle long-running or resource-intensive operations in an asynchronous manner, allowing your web server to efficiently handle multiple requests at the same time. Instead of blocking the thread while waiting for a resource-intensive task (e.g., file I/O, external API requests, or database queries) to complete, asynchronous processing releases the thread to handle other incoming requests.

This approach improves the **scalability** and **performance** of your application, as it allows multiple requests to be processed concurrently without waiting for each request to finish. In .NET Core, asynchronous processing can be achieved by making use of `async` and `await` keywords in combination with non-blocking I/O operations.

### Why Use Asynchronous Processing?

1.  **Increased Scalability**: Allows your application to handle more concurrent requests without overloading server threads.
2.  **Improved Performance**: Non-blocking operations make it possible to handle multiple tasks simultaneously, reducing the overall response time.
3.  **Efficient Resource Usage**: Reduces thread contention, allowing the system to use its resources more efficiently.

### Asynchronous Processing Middleware Implementation in .NET Core

In this example, we will create an **Asynchronous Processing Middleware** that simulates a long-running task (like waiting for a response from an external API or performing a database operation) asynchronously. The middleware will allow the application to continue processing other requests while waiting for the task to finish.

### Explanation:

1.  **Middleware Logic**:
    
    -   This middleware simulates a long-running task using `Task.Delay(5000)` to represent a non-blocking, asynchronous operation.
    -   The middleware logs the start and end of the task, allowing you to monitor the flow of the request.
2.  **Asynchronous Task**:
    
    -   The method `SimulateLongRunningTaskAsync()` is an asynchronous method that simulates a delay of 5 seconds (which could represent a real-world operation like fetching data from an external service or waiting for a database query to complete).
3.  **Next Middleware**:
    
    -   After completing the asynchronous task, the middleware calls `_next(context)` to pass the request to the next middleware in the pipeline, ensuring that the request continues its journey through the pipeline without blocking.

### How to Use the Middleware:

#### Step 1: Register the Middleware in `Startup.cs`

Add the asynchronous processing middleware in the `Configure` method of `Startup.cs`:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Use asynchronous processing middleware
    app.UseAsynchronousProcessing();

    app.UseRouting();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

```

#### Step 2: Simulating Requests in Your Controller

You can test the asynchronous behavior by adding a simple controller that logs when it starts and finishes processing:
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // Simulate a delay to show that the request is not blocked
        _logger.LogInformation("Processing request for /api/products");
        await Task.Delay(3000);  // Simulate some processing time (3 seconds)
        _logger.LogInformation("Finished processing request for /api/products");

        return Ok(new { Message = "Products fetched successfully" });
    }
}

```

### How it Works:

1.  **Request for `/api/products`**: When a client makes a request, the **AsynchronousProcessingMiddleware** will simulate a long-running task (5 seconds) without blocking the server thread.
2.  **Logging**: The middleware logs the start and end of the asynchronous task.
3.  **Controller**: Once the middleware completes its task, the request is passed on to the `ProductsController`, which simulates additional processing (3 seconds).
4.  **Response**: After the controller finishes processing, the response is returned to the client.

### Example Request and Response:

-   **Request**: `GET /api/products`
-   **Logs**:
    -   "Starting asynchronous processing for request: /api/products"
    -   "Asynchronous processing completed for request: /api/products"
    -   "Processing request for /api/products"
    -   "Finished processing request for /api/products"
-   **Response**: `{ "Message": "Products fetched successfully" }`

### Advanced Features and Customization:

1.  **Handling Real-World Asynchronous Tasks**:
    
    -   Instead of using `Task.Delay()`, you can perform real-world async operations, like querying a database, fetching data from an external API, or performing file I/O asynchronously.
2.  **Parallel Processing**:
    
    -   You can extend this middleware to run multiple asynchronous tasks in parallel (using `Task.WhenAll()` or similar techniques) to optimize the time spent on multiple tasks.
3.  **Timeout Handling**:
    
    -   For long-running tasks, you might want to add timeout logic to ensure that requests are not delayed indefinitely. You can use `CancellationToken` to cancel the asynchronous operations if they take too long.
4.  **Error Handling**:
    
    -   Add error-handling logic to catch any exceptions during asynchronous processing, such as timeouts or external service failures. This can be done using try-catch blocks around your asynchronous operations.

### Conclusion:

This **Asynchronous Processing Middleware** allows you to handle long-running tasks without blocking the server thread, improving the scalability and performance of your application. The example provided simulates a long-running task, but in a real application, this middleware can be adapted to handle tasks such as database queries, file I/O, or calls to external services asynchronously. This approach enhances the responsiveness of your API, ensuring that multiple requests can be processed concurrently without being delayed by slow operations.



# PublicFunction.Middleware.WebSocketMiddleware

### WebSocket Middleware Explanation

**WebSocket** is a protocol that provides full-duplex communication channels over a single, long-lived TCP connection. It enables real-time, two-way interaction between a client (such as a browser) and a server, which is useful for applications that require live data, such as chat applications, real-time notifications, or live updates.

WebSocket is different from the traditional HTTP protocol because it allows the client and server to send data to each other at any time without the need for multiple HTTP requests and responses. Once a WebSocket connection is established, both the client and server can send messages to each other without waiting for the other to request data.

### Why Use WebSocket Middleware?

1.  **Real-Time Communication**: WebSocket allows for real-time, interactive communication between the server and the client.
2.  **Efficient**: Since WebSocket maintains a persistent connection, there is no need to repeatedly establish new connections (as in HTTP requests), reducing overhead.
3.  **Low Latency**: WebSocket is ideal for applications that require low-latency communication, such as chat applications, online gaming, and live data feeds.
4.  **Bidirectional Communication**: Both the server and client can send data to each other simultaneously.

### WebSocket Middleware Implementation in .NET Core

In this example, we will implement **WebSocket Middleware** to handle WebSocket connections and send/receive messages.

### Explanation:

1.  **WebSocket Request Handling**:
    
    -   The `InvokeAsync` method checks if the incoming request is a WebSocket request using `context.WebSockets.IsWebSocketRequest`. If it's not a WebSocket request, the request is passed to the next middleware in the pipeline.
    -   If it's a WebSocket request, the connection is accepted via `AcceptWebSocketAsync()`, and the connection is passed to `HandleWebSocketCommunication()` to manage the WebSocket interaction.
2.  **Handling WebSocket Communication**:
    
    -   The `HandleWebSocketCommunication` method continuously reads messages from the WebSocket connection using `ReceiveAsync` and writes responses back to the client using `SendAsync`.
    -   The `ReceiveAsync` method receives data from the WebSocket. If the message type is `Text`, it processes the message and sends an echo back to the client. If the message type is `Close`, the connection is closed.
3.  **Error Handling**:
    
    -   Errors that occur during WebSocket communication are logged with the `ILogger`, and the WebSocket connection is gracefully closed if an exception occurs.
4.  **Buffering**:
    
    -   A buffer of size `1024 * 4` is used for receiving data. You can adjust the buffer size based on your application’s needs.

### How to Use the WebSocket Middleware:

#### Step 1: Register the WebSocket Middleware in `Startup.cs`

In the `Configure` method of `Startup.cs`, enable WebSocket support and register the middleware:
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Enable WebSocket support
    var options = new WebSocketOptions
    {
        KeepAliveInterval = TimeSpan.FromSeconds(120),  // Set the WebSocket keep-alive interval
        ReceiveBufferSize = 1024 * 4  // Buffer size
    };

    app.UseWebSockets(options);

    // Use WebSocket middleware
    app.UseWebSocketMiddleware();

    app.UseRouting();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

```

#### Step 2: Creating WebSocket Clients

You can test your WebSocket implementation by creating a simple WebSocket client. Here's an example using JavaScript in a browser:
```html
<!DOCTYPE html>
<html>
<head>
    <title>WebSocket Client</title>
    <script>
        var socket = new WebSocket("ws://localhost:5000/api/socket");

        // When WebSocket connection is established
        socket.onopen = function(event) {
            console.log("WebSocket connection established");
            socket.send("Hello from client!");
        };

        // When receiving a message from the server
        socket.onmessage = function(event) {
            console.log("Message from server: ", event.data);
        };

        // When WebSocket connection is closed
        socket.onclose = function(event) {
            console.log("WebSocket connection closed");
        };
    </script>
</head>
<body>
    <h1>WebSocket Client</h1>
</body>
</html>

```
In the above example, the client connects to the WebSocket server at `ws://localhost:5000/api/socket`. Once the connection is open, it sends a message to the server, and the server echoes the message back.

### Example Request and Response:

-   **Request**: Client sends the message `"Hello from client!"`.
-   **Server Response**: Server sends the message `"Echo: Hello from client!"` back to the client.

### Advanced Features and Customization:

1.  **Broadcasting Messages**:
    
    -   You can extend the middleware to broadcast messages to all connected clients. Maintain a list of active WebSocket connections and send messages to all clients when one of them sends a message.
2.  **Message Validation**:
    
    -   Add message validation or filtering to ensure only valid messages are processed, or reject malicious content or malformed data.
3.  **Authentication and Authorization**:
    
    -   You can apply authentication or authorization checks before accepting the WebSocket connection. For instance, you could check that the client has a valid token before establishing the WebSocket connection.
4.  **Graceful Shutdown**:
    
    -   Implement a graceful shutdown of WebSocket connections when the server shuts down. This ensures that clients receive a proper closure message and the connection is cleanly closed.

### Conclusion:

This **WebSocket Middleware** allows for real-time, two-way communication between clients and the server using the WebSocket protocol. It’s ideal for use cases that require low-latency communication, such as live updates, chat applications, or real-time data feeds. The implementation shown provides basic WebSocket handling, and you can extend it to include features like broadcasting, message validation, or even secure WebSocket connections using SSL/TLS.


# PublicFunction.Middleware.SignalRMiddleware
### SignalR Middleware Explanation

**SignalR** is a .NET library that simplifies adding real-time web functionality to your applications. Real-time functionality means that the server can push content to connected clients instantly as it becomes available, without the client having to request it. SignalR enables bi-directional communication between server and client, allowing features such as live notifications, chat systems, real-time updates, and more.

SignalR abstracts many of the complexities associated with real-time communication protocols (like WebSockets) and provides fallback mechanisms for environments where WebSockets are not available, using technologies such as **Long Polling**, **Server-Sent Events**, or **Forever Frame**.

SignalR is widely used in applications like:

-   **Live chat** applications
-   **Online gaming** applications
-   **Real-time notifications** (e.g., push notifications in a social network)
-   **Collaborative applications** (e.g., shared document editing)

### Why Use SignalR Middleware?

1.  **Real-Time Communication**: Send updates to clients in real-time without the need for polling.
2.  **Simplified Implementation**: SignalR abstracts the complexities of dealing with WebSockets, server-sent events, or long polling.
3.  **Broad Browser Support**: SignalR automatically chooses the best transport method for the client's environment, ensuring compatibility with many different browsers and devices.
4.  **Scalable**: SignalR supports the ability to scale out, so you can add more servers to handle more clients.

### SignalR Middleware Implementation in .NET Core

To use **SignalR Middleware**, we will first implement SignalR in the application, define a `Hub` (a central point for communication), and integrate SignalR into the pipeline via middleware.

### Step-by-Step Guide to Implement SignalR Middleware:

#### Step 1: Install SignalR NuGet Package

First, ensure that you have the SignalR package installed in your project.

Run the following command in the Package Manager Console:
```bash
dotnet add package Microsoft.AspNetCore.SignalR
```

#### Step 2: Define a SignalR Hub

A **Hub** is a class that clients can call methods on, and that can send messages to clients. It’s the core of real-time communication in SignalR.
```csharp
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        // Sends a message to all connected clients
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}

```
### Step 3: Configure SignalR in `Startup.cs`

In the `Configure` method of `Startup.cs`, you need to register SignalR and use the SignalR middleware.
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Add SignalR services to the DI container
    services.AddSignalR();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Use SignalR middleware
    app.UseSignalRMiddleware();

    // Other middleware configurations
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        // Map the SignalR Hub route
        endpoints.MapHub<ChatHub>("/chatHub");
    });
}

```
### Step 4: Implement Client-Side JavaScript for SignalR

You can interact with SignalR from the client side using JavaScript. Here’s an example of how a simple chat client can send and receive messages via SignalR:
```html
<!DOCTYPE html>
<html>
<head>
    <title>SignalR Chat</title>
    <script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr@3.1.8/dist/browser/signalr.min.js"></script>
    <script>
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/chatHub")
            .build();

        // Receive messages from the server
        connection.on("ReceiveMessage", function (user, message) {
            const msg = `${user}: ${message}`;
            const li = document.createElement("li");
            li.textContent = msg;
            document.getElementById("messagesList").appendChild(li);
        });

        // Start the connection
        async function start() {
            try {
                await connection.start();
                console.log("SignalR connected!");
            } catch (err) {
                console.error(err);
                setTimeout(() => start(), 5000);
            }
        }

        connection.onclose(() => start());

        // Send message to server
        function sendMessage() {
            const user = document.getElementById("userInput").value;
            const message = document.getElementById("messageInput").value;
            connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
            document.getElementById("messageInput").value = '';
        }

        start();
    </script>
</head>
<body>
    <ul id="messagesList"></ul>
    <input type="text" id="userInput" placeholder="Enter your name" />
    <input type="text" id="messageInput" placeholder="Enter message" />
    <button onclick="sendMessage()">Send</button>
</body>
</html>

```
### Explanation:

-   **SignalR Hub**: The `ChatHub` class defines a method `SendMessage` that sends messages to all connected clients using `Clients.All.SendAsync`.
-   **Middleware**: The `SignalRMiddleware` logs each request that passes through it, providing a basic example of how you can handle SignalR-specific requests in middleware. This middleware is optional, but it can be useful for logging or other pre-processing.
-   **Client-Side**: The JavaScript code on the client side connects to the SignalR Hub and listens for messages. It sends a message to the Hub when the user submits a message through the form.

### How to Use the Middleware:

1.  **WebSocket and HTTP Transport**: SignalR will automatically use WebSockets if they are supported in the client and server. If WebSockets are not available, it will fall back to other transport mechanisms such as Long Polling.
2.  **SignalR Connection**: On the client side, the connection to the SignalR hub is created using `new signalR.HubConnectionBuilder().withUrl("/chatHub")`, and you can start the connection with `await connection.start()`.

### Example Request and Response:

-   **Client Message**: User sends a message like `"Hello, world!"`.
-   **Server Action**: The `SendMessage` method on the server is invoked, and the message is broadcast to all connected clients.
-   **Client Response**: All connected clients receive the message in real time and append it to their message list.

### Advanced Features and Customization:

1.  **Authentication and Authorization**: You can apply authentication and authorization to SignalR hubs to restrict access to certain users or roles. For example:

```csharp
public class ChatHub : Hub
{
    public override Task OnConnectedAsync()
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("You must be authenticated to connect.");
        }

        return base.OnConnectedAsync();
    }
}

```
2. **Group Management**: SignalR supports grouping users into groups. This allows you to send messages to a subset of clients rather than all clients.
```csharp
public async Task AddToGroup(string groupName)
{
    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
}
```
3.   **Broadcasting to Specific Clients**: You can broadcast messages to specific clients or groups using `Clients.Group`, `Clients.Client`, or `Clients.User` for more targeted communication.
    

### Conclusion:

**SignalR Middleware** allows you to implement real-time web functionality easily in your ASP.NET Core application. It enables two-way communication between server and client without the overhead of repeated HTTP requests. With features like automatic transport selection, group management, and real-time message broadcasting, SignalR is powerful for building applications that require instant updates and live data feeds.

