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
