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
