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
