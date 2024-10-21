### Class Explanation: `IntegerService`

The `IntegerService` class is designed to provide methods for converting integers to various types and vice versa. It implements the `IInteger` interface, which defines three main functionalities:

1.  **ConvertTo<T>**: Converts an integer to a specified target type `T`.
2.  **ConvertFrom<T>**: Converts an object of type `T` to an integer.
3.  **TryConvertTo**: Attempts to convert a string input to an integer, returning a boolean indicating success or failure.

### Detailed Breakdown of Methods:

#### 1. ConvertTo<T>(int input)

-   **Purpose**: Converts the integer `input` to the specified type `T`.
-   **Supported Conversions**:
    -   `string`: Returns the integer as a string.
    -   `double`, `decimal`, `float`: Converts the integer to the respective floating-point type.
    -   `bool`: Returns `true` if the integer is non-zero; otherwise, returns `false`.
    -   Uses `Convert.ChangeType` as a fallback for other types.
-   **Exceptions**: Throws `InvalidOperationException` if the conversion fails or if the type is not supported.

#### 2. ConvertFrom<T>(T input)

-   **Purpose**: Converts an object of type `T` to an integer.
-   **Supported Types**:
    -   Handles conversions from `int`, `string`, `double`, `decimal`, and `float`.
-   **Return Value**: Returns the integer representation of the input.
-   **Exceptions**: Throws `InvalidOperationException` for unsupported types or conversion failures.

#### 3. TryConvertTo(string input, out int result)

-   **Purpose**: Attempts to convert a string to an integer.
-   **Parameters**:
    -   `input`: The string to convert.
    -   `result`: An output parameter that holds the converted integer if successful; otherwise, it is set to `0`.
-   **Return Value**: Returns `true` if the conversion is successful, otherwise returns `false`.

### Usage Example

Here's how you can use the `IntegerService` class in a C# application:

```csharp
class Program
{
    static void Main()
    {
        Integer.IInteger integerService = new Integer.IntegerService();

        // Example of converting int to string
        int number = 42;
        string numberAsString = integerService.ConvertTo<string>(number);
        Console.WriteLine($"Integer to String: {numberAsString}"); // Output: "42"

        // Example of converting int to double
        double numberAsDouble = integerService.ConvertTo<double>(number);
        Console.WriteLine($"Integer to Double: {numberAsDouble}"); // Output: "42"

        // Example of converting int to bool
        bool numberAsBool = integerService.ConvertTo<bool>(number);
        Console.WriteLine($"Integer to Bool: {numberAsBool}"); // Output: "True"

        // Example of converting string to int
        string inputString = "123";
        if (integerService.TryConvertTo(inputString, out int convertedInt))
        {
            Console.WriteLine($"String to Integer: {convertedInt}"); // Output: "123"
        }
        else
        {
            Console.WriteLine("Failed to convert string to integer.");
        }

        // Example of converting double to int
        double inputDouble = 99.99;
        int intFromDouble = integerService.ConvertFrom(inputDouble);
        Console.WriteLine($"Double to Integer: {intFromDouble}"); // Output: "99"
    }
}

```
