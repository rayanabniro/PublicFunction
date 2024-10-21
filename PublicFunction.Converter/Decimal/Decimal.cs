using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter
{
    public class Decimal
    {
        public interface IDecimal
        {
            T ConvertTo<T>(decimal input);
            decimal ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out decimal result);
        }

        public class DecimalService : IDecimal
        {
            /// <summary>
            /// Converts a decimal input to the specified type T.
            /// Supports conversions to string, int, double, bool, and others.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The decimal input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(decimal input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // If the target type is string, return the decimal as a string
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (targetType == typeof(int))
                    {
                        return (T)(object)Convert.ToInt32(input); // Convert to int
                    }
                    else if (targetType == typeof(double))
                    {
                        return (T)(object)Convert.ToDouble(input); // Convert to double
                    }
                    else if (targetType == typeof(bool))
                    {
                        return (T)(object)(input != 0); // Convert non-zero decimal to true, zero to false
                    }
                    else if (targetType.IsEnum)
                    {
                        return (T)Enum.ToObject(targetType, Convert.ToInt32(input)); // Convert to enum based on decimal
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
            /// Converts an object of type T to its decimal representation.
            /// Supports string, int, double, bool, and other types.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to decimal.</param>
            /// <returns>The decimal representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public decimal ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        return 0m; // Default value for decimal
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        return decimal.Parse((string)(object)input, CultureInfo.InvariantCulture); // Parse string to decimal
                    }
                    else if (underlyingType == typeof(int))
                    {
                        return Convert.ToDecimal((int)(object)input); // Convert int to decimal
                    }
                    else if (underlyingType == typeof(double))
                    {
                        return Convert.ToDecimal((double)(object)input); // Convert double to decimal
                    }
                    else if (underlyingType == typeof(bool))
                    {
                        return (bool)(object)input ? 1m : 0m; // Convert bool to decimal (true to 1, false to 0)
                    }
                    else
                    {
                        return Convert.ToDecimal(input, CultureInfo.InvariantCulture); // Fallback to Convert
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to decimal.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a decimal.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of decimal.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out decimal result)
            {
                return decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            }
        }
    }
}
