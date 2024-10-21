using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter.Long
{
    public class Long
    {
        public interface ILong
        {
            T ConvertTo<T>(long input);
            long ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out long result);
        }

        public class LongService : ILong
        {
            /// <summary>
            /// Converts a long input to the specified type T.
            /// Supports conversions to string, int, double, bool, and others.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The long input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(long input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // If the target type is string, return the long as a string
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
                        return (T)(object)(input != 0); // Convert non-zero long to true, zero to false
                    }
                    else if (targetType.IsEnum)
                    {
                        return (T)Enum.ToObject(targetType, input); // Convert to enum based on long
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
            /// Converts an object of type T to its long representation.
            /// Supports string, int, double, bool, and other types.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to long.</param>
            /// <returns>The long representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public long ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        return 0; // Default value for long
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        return long.Parse((string)(object)input, CultureInfo.InvariantCulture); // Parse string to long
                    }
                    else if (underlyingType == typeof(int))
                    {
                        return Convert.ToInt64((int)(object)input); // Convert int to long
                    }
                    else if (underlyingType == typeof(double))
                    {
                        return Convert.ToInt64((double)(object)input); // Convert double to long
                    }
                    else if (underlyingType == typeof(bool))
                    {
                        return (bool)(object)input ? 1 : 0; // Convert bool to long (true to 1, false to 0)
                    }
                    else
                    {
                        return Convert.ToInt64(input, CultureInfo.InvariantCulture); // Fallback to Convert
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to long.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a long.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of long.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out long result)
            {
                return long.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            }
        }
    }
}
