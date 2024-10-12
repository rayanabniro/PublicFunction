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
