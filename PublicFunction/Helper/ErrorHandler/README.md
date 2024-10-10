### `ErrorHandler` Class and Interface for Localization

This C# class and interface setup is designed to manage error messages across multiple languages. It allows adding and retrieving localized error messages based on exceptions, making your application more user-friendly.

### Key Components:

-   **`IErrorHandler`**: Defines core methods like `AddError`, `GetErrorMessage`, etc.
-   **`ErrorHandler`**: Implements the methods to store, retrieve, and manage error translations.

### Example Usage:
```csharp
var handler = new ErrorHandler();

// Add English error message
handler.AddError(Language.en, new List<Error>
{
    new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Object reference not set to an instance of an object." }
});

// Add French error message
handler.AddError(Language.fr, new List<Error>
{
    new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Référence d'objet non définie à une instance d'un objet." }
});

// Retrieve message in French
var message = handler.GetErrorMessage(Language.fr, new NullReferenceException());
Console.WriteLine(message);  // Outputs localized message in French

```
### Benefits:

-   **Localization**: Supports multiple languages, improving user experience.
-   **Centralized Error Management**: Makes it easier to manage and retrieve error messages from a single source.
-   **Extensibility**: You can easily add more languages and error types over time.

This approach ensures better maintainability and clarity in applications handling multiple languages.

# To inject and use
To inject and use the `ErrorHandler` in a .NET project, you can implement **dependency injection**. Dependency Injection (DI) is commonly used in .NET to manage the lifecycle and dependencies of objects.

Here’s how to do it:

### 1. Configure the Service in `Startup.cs` or `Program.cs`

Register the `ErrorHandler` class as a service in your application so that it can be injected into other classes.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IErrorHandler, ErrorHandler>();
}

```
-   **`AddSingleton`**: Ensures that the same instance of `ErrorHandler` is used throughout the app. You could use `AddScoped` if you want an instance per request or `AddTransient` for new instances every time.

### 2. Inject `ErrorHandler` into a Controller or Service

You can inject the `IErrorHandler` in any controller or service where you need to manage error messages.

```csharp
public class HomeController : Controller
{
    private readonly IErrorHandler _errorHandler;

    public HomeController(IErrorHandler errorHandler)
    {
        _errorHandler = errorHandler;
    }

    public IActionResult Index()
    {
        // Add a new error in English
        _errorHandler.AddError(Language.en, new List<Error>
        {
            new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Value cannot be null." }
        });

        // Get error message in English
        var errorMessage = _errorHandler.GetErrorMessage(Language.en, new ArgumentNullException());
        
        // Use the error message for logging or displaying to the user
        ViewBag.ErrorMessage = errorMessage;

        return View();
    }
}

```
### Explanation of Usage:

1.  **Injection**: The `IErrorHandler` is injected into the `HomeController` via constructor injection, which is possible because the service was registered in the `ConfigureServices` method.
2.  **Error Management**: In this example, the error message for an `ArgumentNullException` is added in English, and later it's retrieved in the `Index` action of the controller.
3.  **Access and Usage**: Once injected, the `_errorHandler` can be used throughout the controller to add, retrieve, or clean up error translations.

### Benefits of Using Dependency Injection:

-   **Separation of Concerns**: Error management is centralized in a single service (`ErrorHandler`), making your controllers and services cleaner and focused on their responsibilities.
-   **Testability**: Using interfaces (`IErrorHandler`), you can easily mock the error handler service for unit tests.
-   **Extensibility**: You can extend or modify the error-handling logic (like logging or alerting) in the `ErrorHandler` class without affecting other parts of your codebase.

### Example in Action:

In this case, whenever an exception occurs in the controller, the corresponding localized error message is fetched from the `ErrorHandler` and displayed to the user.
