### Class Overview

1.  **IUShort Interface**: This interface defines the methods for converting `ushort` values to various types and converting other types to `ushort`.
    
2.  **ConvertTo<T> Method**: This method converts a `ushort` input to a specified type `T`. It supports conversions to common types like `string`, `int`, `long`, `double`, and `float`.
    
3.  **ConvertFrom<T> Method**: This method converts an object of type `T` to a `ushort`. It supports conversions from `string`, `int`, `long`, `double`, and `float`, ensuring that the values are within the valid range for `ushort`.
    
4.  **TryConvertTo Method**: This method attempts to convert a string input to a `ushort`, returning a boolean indicating the success of the operation.
    

### Usage Example

Here’s how you can use the `UShortService` class in a C# application:

```csharp
class Program
{
    static void Main()
    {
        IUShort ushortService = new UShortService();

        // Example of converting ushort to string
        ushort ushortValue = 12345;
        string ushortAsString = ushortService.ConvertTo<string>(ushortValue);
        Console.WriteLine($"UShort to String: {ushortAsString}");

        // Example of converting ushort to int
        int ushortAsInt = ushortService.ConvertTo<int>(ushortValue);
        Console.WriteLine($"UShort to Int: {ushortAsInt}");

        // Example of converting string to ushort
        string inputString = "54321";
        if (ushortService.TryConvertTo(inputString, out ushort convertedUshort))
        {
            Console.WriteLine($"String to UShort: {convertedUshort}");
        }
        else
        {
            Console.WriteLine("Failed to convert string to ushort.");
        }

        // Example of converting int to ushort
        ushort ushortFromInt = ushortService.ConvertFrom(123);
        Console.WriteLine($"Int to UShort: {ushortFromInt}");
    }
}

```
