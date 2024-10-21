using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter.ULong
{
    public class ULong
    {
        public interface IULong
        {
            T ConvertTo<T>(ulong input);
            ulong ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out ulong result);
        }

        public class ULongService : IULong
        {
            /// <summary>
            /// Converts a ulong input to the specified type T.
            /// Supports conversions to string, long, double, etc.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The ulong input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(ulong input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // Handle conversions to common types
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString(); // Convert ulong to string
                    }
                    else if (targetType == typeof(long))
                    {
                        if (input > long.MaxValue)
                        {
                            throw new InvalidOperationException($"Cannot convert {input} to long as it exceeds the maximum value.");
                        }
                        return (T)(object)(long)input; // Convert ulong to long
                    }
                    else if (targetType == typeof(double))
                    {
                        return (T)(object)(double)input; // Convert ulong to double
                    }
                    else if (targetType == typeof(float))
                    {
                        return (T)(object)(float)input; // Convert ulong to float
                    }
                    else if (targetType == typeof(int))
                    {
                        if (input > int.MaxValue)
                        {
                            throw new InvalidOperationException($"Cannot convert {input} to int as it exceeds the maximum value.");
                        }
                        return (T)(object)(int)input; // Convert ulong to int
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
            /// Converts an object of type T to its ulong representation.
            /// Supports conversions from string, long, int, etc.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to ulong.</param>
            /// <returns>The ulong representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public ulong ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        throw new InvalidOperationException("Cannot convert null to ulong.");
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        return ulong.Parse((string)(object)input, CultureInfo.InvariantCulture); // Parse string to ulong
                    }
                    else if (underlyingType == typeof(long))
                    {
                        if ((long)(object)input < 0)
                        {
                            throw new InvalidOperationException($"Cannot convert negative long value to ulong.");
                        }
                        return Convert.ToUInt64(input); // Convert long to ulong
                    }
                    else if (underlyingType == typeof(int))
                    {
                        return Convert.ToUInt64(input); // Convert int to ulong
                    }
                    else if (underlyingType == typeof(double))
                    {
                        return Convert.ToUInt64(input); // Convert double to ulong
                    }
                    else if (underlyingType == typeof(float))
                    {
                        return Convert.ToUInt64(input); // Convert float to ulong
                    }
                    else
                    {
                        throw new InvalidOperationException($"Conversion from {inputType.Name} to ulong is not supported.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to ulong.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a ulong.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of ulong.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out ulong result)
            {
                return ulong.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out result);
            }
        }
    }
}
