### Class Overview

1.  **IDateTime Interface**: This interface defines the methods for converting `DateTime` values to various types and converting other types to `DateTime`.
    
2.  **ConvertTo<T> Method**: This method converts a `DateTime` input to a specified type `T`. It supports conversions to common types like `string`, `int`, `double`, `bool`, and enumerations.
    
3.  **ConvertFrom<T> Method**: This method converts an object of type `T` to a `DateTime`. It supports conversions from `string`, `int`, `double`, and `bool`.
    
4.  **TryConvertTo Method**: This method attempts to convert a string input to a `DateTime`, returning a boolean indicating the success of the operation.
    

### Usage Example

Here’s how you can use the `DateTimeService` class in a C# application:
```csharp
class Program
{
    static void Main()
    {
        IDateTime dateTimeService = new DateTimeService();

        // Example of converting DateTime to string
        DateTime now = DateTime.Now;
        string nowAsString = dateTimeService.ConvertTo<string>(now);
        Console.WriteLine($"DateTime to String: {nowAsString}"); // Output: ISO 8601 formatted string

        // Example of converting DateTime to int (Unix timestamp)
        int nowAsUnixTimestamp = dateTimeService.ConvertTo<int>(now);
        Console.WriteLine($"DateTime to Unix Timestamp: {nowAsUnixTimestamp}");

        // Example of converting DateTime to double (Unix timestamp)
        double nowAsUnixTimestampDouble = dateTimeService.ConvertTo<double>(now);
        Console.WriteLine($"DateTime to Unix Timestamp (double): {nowAsUnixTimestampDouble}");

        // Example of converting string to DateTime
        string inputString = "2024-10-10T12:00:00";
        if (dateTimeService.TryConvertTo(inputString, out DateTime convertedDateTime))
        {
            Console.WriteLine($"String to DateTime: {convertedDateTime}"); // Output: Converted DateTime
        }
        else
        {
            Console.WriteLine("Failed to convert string to DateTime.");
        }

        // Example of converting int (Unix timestamp) to DateTime
        int unixTimestamp = 1696934400; // Example Unix timestamp
        DateTime dateTimeFromUnix = dateTimeService.ConvertFrom(unixTimestamp);
        Console.WriteLine($"Unix Timestamp to DateTime: {dateTimeFromUnix}");
    }
}

```
