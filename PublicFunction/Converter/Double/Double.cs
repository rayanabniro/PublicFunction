using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter
{
    public class Double
    {
        public interface IDouble
        {
            T ConvertTo<T>(double input);
            double ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out double result);
        }

        public class DoubleService : IDouble
        {
            /// <summary>
            /// Converts a double input to the specified type T.
            /// Supports basic types, nullable types, and enums.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The double input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(double input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // If the target type is string, return the input as string
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString(CultureInfo.InvariantCulture);
                    }

                    // Handle specific types
                    if (targetType == typeof(int))
                    {
                        return (T)(object)(int)input;
                    }
                    else if (targetType == typeof(decimal))
                    {
                        return (T)(object)(decimal)input;
                    }
                    else if (targetType == typeof(float))
                    {
                        return (T)(object)(float)input;
                    }
                    else if (targetType == typeof(bool))
                    {
                        return (T)(object)(input != 0); // Non-zero is true, zero is false
                    }
                    else if (targetType == typeof(DateTime))
                    {
                        // Example: Consider the double input as a Unix timestamp (seconds since 1970-01-01)
                        return (T)(object)(new DateTime(1970, 1, 1).AddSeconds(input));
                    }
                    else if (targetType.IsEnum)
                    {
                        return (T)Enum.ToObject(targetType, (int)input);
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
            /// Converts an object of type T to its double representation.
            /// Supports basic types and nullable types.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to double.</param>
            /// <returns>The double representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public double ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        return 0.0;
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(int))
                    {
                        return Convert.ToDouble(input);
                    }
                    else if (underlyingType == typeof(decimal))
                    {
                        return (double)(decimal)(object)input;
                    }
                    else if (underlyingType == typeof(float))
                    {
                        return (double)(float)(object)input;
                    }
                    else if (underlyingType == typeof(string))
                    {
                        if (double.TryParse((string)(object)input, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
                        {
                            return result;
                        }
                        throw new InvalidOperationException($"Failed to convert string to double: {input}.");
                    }
                    else if (underlyingType == typeof(bool))
                    {
                        return ((bool)(object)input) ? 1.0 : 0.0;
                    }
                    else if (underlyingType == typeof(DateTime))
                    {
                        return ((DateTime)(object)input).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    }
                    else
                    {
                        return Convert.ToDouble(input, CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to double.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a double.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, 0.0.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out double result)
            {
                return double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
            }
        }
    }
}
