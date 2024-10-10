using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter
{
    public class String
    {
        public interface IString
        {
            /// <summary>
            /// Converts a string input to the specified type T.
            /// Supports basic types, nullable types, enums, and more.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The string input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            T ConvertTo<T>(string input);

            /// <summary>
            /// Converts an object of type T to its string representation.
            /// Supports basic types, nullable types, enums, and more.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to string.</param>
            /// <returns>The string representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            string ConvertFrom<T>(T input);

            /// <summary>
            /// Attempts to convert a string input to the specified type T.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of T.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            bool TryConvertTo<T>(string input, out T result);
        }
        public class StringService : IString
        {
            /// <summary>
            /// Converts a string input to the specified type T.
            /// Supports basic types, nullable types, enums, and more.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The string input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(string input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // Handle nullable types
                    Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                    // If the target type is string, return the input directly
                    if (underlyingType == typeof(string))
                    {
                        return (T)(object)input;
                    }

                    // If input is null or whitespace, return default value for nullable types
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        if (Nullable.GetUnderlyingType(targetType) != null)
                        {
                            return default;
                        }
                        throw new InvalidOperationException($"Cannot convert null or empty string to non-nullable type {underlyingType.Name}.");
                    }

                    // Handle enums
                    if (underlyingType.IsEnum)
                    {
                        return (T)Enum.Parse(underlyingType, input, ignoreCase: true);
                    }

                    // Handle specific types
                    if (underlyingType == typeof(int))
                    {
                        return (T)(object)int.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(double))
                    {
                        return (T)(object)double.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(DateTime))
                    {
                        return (T)(object)DateTime.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(bool))
                    {
                        return (T)(object)bool.Parse(input);
                    }
                    else if (underlyingType == typeof(decimal))
                    {
                        return (T)(object)decimal.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(long))
                    {
                        return (T)(object)long.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(float))
                    {
                        return (T)(object)float.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(Guid))
                    {
                        return (T)(object)Guid.Parse(input);
                    }
                    else if (underlyingType == typeof(byte))
                    {
                        return (T)(object)byte.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(short))
                    {
                        return (T)(object)short.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(uint))
                    {
                        return (T)(object)uint.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(ulong))
                    {
                        return (T)(object)ulong.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(ushort))
                    {
                        return (T)(object)ushort.Parse(input, CultureInfo.InvariantCulture);
                    }
                    else if (underlyingType == typeof(char))
                    {
                        if (input.Length != 1)
                            throw new InvalidOperationException($"Cannot convert string '{input}' to char. Input string must be exactly one character.");
                        return (T)(object)input[0];
                    }
                    else
                    {
                        // Use Convert.ChangeType as a fallback for other types
                        return (T)Convert.ChangeType(input, underlyingType, CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert '{input}' to {typeof(T).Name}.", ex);
                }
            }

            /// <summary>
            /// Converts an object of type T to its string representation.
            /// Supports basic types, nullable types, enums, and more.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to string.</param>
            /// <returns>The string representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public string ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        return string.Empty;
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(DateTime))
                    {
                        return ((DateTime)(object)input).ToString("o", CultureInfo.InvariantCulture); // ISO 8601 format
                    }
                    else if (underlyingType.IsEnum)
                    {
                        return input.ToString();
                    }
                    else if (underlyingType == typeof(bool))
                    {
                        return ((bool)(object)input).ToString().ToLower(); // "true" or "false"
                    }
                    else if (underlyingType == typeof(char))
                    {
                        return input.ToString();
                    }
                    else
                    {
                        return Convert.ToString(input, CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to string.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to the specified type T.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of T.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo<T>(string input, out T result)
            {
                try
                {
                    result = ConvertTo<T>(input);
                    return true;
                }
                catch
                {
                    result = default;
                    return false;
                }
            }
        }
    }
}

