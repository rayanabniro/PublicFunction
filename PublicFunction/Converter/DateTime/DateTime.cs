using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter.DateTime
{
    public class DateTime
    {
        public interface IDateTime
        {
            T ConvertTo<T>(System.DateTime input);
            System.DateTime ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out System.DateTime result);
        }

        public class DateTimeService : IDateTime
        {
            /// <summary>
            /// Converts a DateTime input to the specified type T.
            /// Supports basic types, nullable types, and enums.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The DateTime input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(System.DateTime input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // If the target type is string, return the input as string
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString("o", CultureInfo.InvariantCulture); // ISO 8601 format
                    }

                    // Handle specific types
                    if (targetType == typeof(int))
                    {
                        // Convert DateTime to Unix timestamp (seconds since 1970-01-01)
                        return (T)(object)(int)(input.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds);
                    }
                    else if (targetType == typeof(double))
                    {
                        // Convert DateTime to Unix timestamp (seconds since 1970-01-01)
                        return (T)(object)input.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
                    }
                    else if (targetType == typeof(bool))
                    {
                        return (T)(object)(input != System.DateTime.MinValue); // Non-MinValue is true
                    }
                    else if (targetType.IsEnum)
                    {
                        return (T)Enum.ToObject(targetType, input.Ticks);
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
            /// Converts an object of type T to its DateTime representation.
            /// Supports basic types and nullable types.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to DateTime.</param>
            /// <returns>The DateTime representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public System.DateTime ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        return System.DateTime.MinValue;
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        if (System.DateTime.TryParse((string)(object)input, CultureInfo.InvariantCulture, DateTimeStyles.None, out System.DateTime result))
                        {
                            return result;
                        }
                        throw new InvalidOperationException($"Failed to convert string to DateTime: {input}.");
                    }
                    else if (underlyingType == typeof(int))
                    {
                        // Convert Unix timestamp (seconds since 1970-01-01) to DateTime
                        return new System.DateTime(1970, 1, 1).AddSeconds((int)(object)input);
                    }
                    else if (underlyingType == typeof(double))
                    {
                        // Convert Unix timestamp (seconds since 1970-01-01) to DateTime
                        return new System.DateTime(1970, 1, 1).AddSeconds((double)(object)input);
                    }
                    else if (underlyingType == typeof(bool))
                    {
                        return (bool)(object)input ? System.DateTime.Now : System.DateTime.MinValue;
                    }
                    else
                    {
                        return Convert.ToDateTime(input, CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to DateTime.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a DateTime.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, DateTime.MinValue.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out System.DateTime result)
            {
                return System.DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            }
        }
    }
}
