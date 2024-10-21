### Class Overview

1.  **ILong Interface**: This interface defines the methods for converting `long` values to various types and converting other types to `long`.
    
2.  **ConvertTo<T> Method**: This method converts a `long` input to a specified type `T`. It supports conversions to common types like `string`, `int`, `double`, and `bool`.
    
3.  **ConvertFrom<T> Method**: This method converts an object of type `T` to a `long`. It supports conversions from `string`, `int`, `double`, and `bool`.
    
4.  **TryConvertTo Method**: This method attempts to convert a string input to a `long`, returning a boolean indicating the success of the operation.
    

### Usage Example

Here’s how you can use the `LongService` class in a C# application:
```csharp
class Program
{
    static void Main()
    {
        ILong longService = new LongService();

        // Example of converting long to string
        long longValue = 1234567890;
        string longAsString = longService.ConvertTo<string>(longValue);
        Console.WriteLine($"Long to String: {longAsString}"); // Output: "1234567890"

        // Example of converting long to int
        int longAsInt = longService.ConvertTo<int>(longValue);
        Console.WriteLine($"Long to Int: {longAsInt}"); // Output: 1234567890

        // Example of converting long to double
        double longAsDouble = longService.ConvertTo<double>(longValue);
        Console.WriteLine($"Long to Double: {longAsDouble}"); // Output: 1234567890

        // Example of converting long to bool
        bool longAsBool = longService.ConvertTo<bool>(longValue);
        Console.WriteLine($"Long to Bool: {longAsBool}"); // Output: True

        // Example of converting string to long
        string inputString = "9876543210";
        if (longService.TryConvertTo(inputString, out long convertedLong))
        {
            Console.WriteLine($"String to Long: {convertedLong}"); // Output: 9876543210
        }
        else
        {
            Console.WriteLine("Failed to convert string to long.");
        }

        // Example of converting int to long
        int inputInt = 200;
        long longFromInt = longService.ConvertFrom(inputInt);
        Console.WriteLine($"Int to Long: {longFromInt}"); // Output: 200
    }
}

```
