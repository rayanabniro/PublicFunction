### Class Overview

1.  **IDouble Interface**: This interface defines the methods for converting `double` values to various types and converting other types to `double`.
    
2.  **ConvertTo<T> Method**: This method converts a `double` input to a specified type `T`. It supports conversions to common types like `string`, `int`, `decimal`, `float`, `bool`, `DateTime`, and enumerations.
    
3.  **ConvertFrom<T> Method**: This method converts an object of type `T` to a `double`. It supports conversions from `int`, `decimal`, `float`, `string`, `bool`, and `DateTime`.
    
4.  **TryConvertTo Method**: This method attempts to convert a string input to a `double`, returning a boolean indicating the success of the operation.
    

### Usage Example

Here’s how you can use the `DoubleService` class in a C# application:
```csharp
class Program
{
    static void Main()
    {
        IDouble doubleService = new DoubleService();

        // Example of converting double to string
        double number = 42.5;
        string numberAsString = doubleService.ConvertTo<string>(number);
        Console.WriteLine($"Double to String: {numberAsString}"); // Output: "42.5"

        // Example of converting double to int
        int numberAsInt = doubleService.ConvertTo<int>(number);
        Console.WriteLine($"Double to Int: {numberAsInt}"); // Output: "42"

        // Example of converting double to bool
        bool numberAsBool = doubleService.ConvertTo<bool>(number);
        Console.WriteLine($"Double to Bool: {numberAsBool}"); // Output: "True"

        // Example of converting string to double
        string inputString = "123.45";
        if (doubleService.TryConvertTo(inputString, out double convertedDouble))
        {
            Console.WriteLine($"String to Double: {convertedDouble}"); // Output: "123.45"
        }
        else
        {
            Console.WriteLine("Failed to convert string to double.");
        }

        // Example of converting int to double
        int inputInt = 99;
        double doubleFromInt = doubleService.ConvertFrom(inputInt);
        Console.WriteLine($"Int to Double: {doubleFromInt}"); // Output: "99.0"
    }
}

```
