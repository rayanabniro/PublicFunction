### Class Overview

1.  **IDecimal Interface**: This interface defines the methods for converting `decimal` values to various types and converting other types to `decimal`.
    
2.  **ConvertTo<T> Method**: This method converts a `decimal` input to a specified type `T`. It supports conversions to common types like `string`, `int`, `double`, and `bool`.
    
3.  **ConvertFrom<T> Method**: This method converts an object of type `T` to a `decimal`. It supports conversions from `string`, `int`, `double`, and `bool`.
    
4.  **TryConvertTo Method**: This method attempts to convert a string input to a `decimal`, returning a boolean indicating the success of the operation.
    

### Usage Example

Here’s how you can use the `DecimalService` class in a C# application:

```csharp
class Program
{
    static void Main()
    {
        IDecimal decimalService = new DecimalService();

        // Example of converting decimal to string
        decimal decimalValue = 12.34m;
        string decimalAsString = decimalService.ConvertTo<string>(decimalValue);
        Console.WriteLine($"Decimal to String: {decimalAsString}"); // Output: "12.34"

        // Example of converting decimal to int
        int decimalAsInt = decimalService.ConvertTo<int>(decimalValue);
        Console.WriteLine($"Decimal to Int: {decimalAsInt}"); // Output: 12

        // Example of converting decimal to double
        double decimalAsDouble = decimalService.ConvertTo<double>(decimalValue);
        Console.WriteLine($"Decimal to Double: {decimalAsDouble}"); // Output: 12.34

        // Example of converting decimal to bool
        bool decimalAsBool = decimalService.ConvertTo<bool>(decimalValue);
        Console.WriteLine($"Decimal to Bool: {decimalAsBool}"); // Output: True

        // Example of converting string to decimal
        string inputString = "45.67";
        if (decimalService.TryConvertTo(inputString, out decimal convertedDecimal))
        {
            Console.WriteLine($"String to Decimal: {convertedDecimal}"); // Output: 45.67
        }
        else
        {
            Console.WriteLine("Failed to convert string to decimal.");
        }

        // Example of converting int to decimal
        int inputInt = 100;
        decimal decimalFromInt = decimalService.ConvertFrom(inputInt);
        Console.WriteLine($"Int to Decimal: {decimalFromInt}"); // Output: 100
    }
}

```
