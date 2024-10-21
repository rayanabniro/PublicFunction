using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter
{
    public class Integer
    {
        public interface IInteger
        {
            public T ConvertTo<T>(int input);
            public int ConvertFrom<T>(T input);
            public bool TryConvertTo(string input, out int result);
        }

        public class IntegerService : IInteger
        {
            /// <summary>
            /// Converts an integer input to the specified type T.
            /// Supports basic types like string, double, and bool.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The integer input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(int input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // Handle specific types
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (targetType == typeof(double))
                    {
                        return (T)(object)(double)input;
                    }
                    else if (targetType == typeof(bool))
                    {
                        return (T)(object)(input != 0); // 0 is false, any other value is true
                    }
                    else if (targetType == typeof(decimal))
                    {
                        return (T)(object)(decimal)input;
                    }
                    else if (targetType == typeof(long))
                    {
                        return (T)(object)(long)input;
                    }
                    else if (targetType == typeof(float))
                    {
                        return (T)(object)(float)input;
                    }
                    else
                    {
                        // Use Convert.ChangeType as a fallback for other types
                        return (T)Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert {input} to {typeof(T).Name}.", ex);
                }
            }

            /// <summary>
            /// Converts an object of type T to its integer representation.
            /// Supports basic types.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to int.</param>
            /// <returns>The integer representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public int ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        return 0; // Return 0 for null inputs
                    }

                    Type inputType = typeof(T);

                    // Handle specific types
                    if (inputType == typeof(int))
                    {
                        return (int)(object)input;
                    }
                    else if (inputType == typeof(string))
                    {
                        return int.Parse((string)(object)input, CultureInfo.InvariantCulture);
                    }
                    else if (inputType == typeof(double))
                    {
                        return (int)(double)(object)input;
                    }
                    else if (inputType == typeof(decimal))
                    {
                        return (int)(decimal)(object)input;
                    }
                    else if (inputType == typeof(float))
                    {
                        return (int)(float)(object)input;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unsupported type: {inputType.Name}");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to int.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to an integer.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted integer value if successful; otherwise, 0.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out int result)
            {
                try
                {
                    result = int.Parse(input, CultureInfo.InvariantCulture);
                    return true;
                }
                catch
                {
                    result = 0; // Default value on failure
                    return false;
                }
            }
        }
    }
}
