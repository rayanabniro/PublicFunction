using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter.Byte
{
    public class Byte
    {
        public interface IByte
        {
            T ConvertTo<T>(byte input);
            byte ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out byte result);
        }

        public class ByteService : IByte
        {
            /// <summary>
            /// Converts a byte input to the specified type T.
            /// Supports conversions to int, string, double, etc.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The byte input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(byte input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // Handle conversions to common types
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString(); // Convert byte to string
                    }
                    else if (targetType == typeof(int))
                    {
                        return (T)(object)(int)input; // Convert byte to int
                    }
                    else if (targetType == typeof(double))
                    {
                        return (T)(object)(double)input; // Convert byte to double
                    }
                    else if (targetType == typeof(short))
                    {
                        return (T)(object)(short)input; // Convert byte to short
                    }
                    else if (targetType == typeof(float))
                    {
                        return (T)(object)(float)input; // Convert byte to float
                    }
                    else if (targetType == typeof(long))
                    {
                        return (T)(object)(long)input; // Convert byte to long
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
            /// Converts an object of type T to its byte representation.
            /// Supports conversions from string, int, etc.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to byte.</param>
            /// <returns>The byte representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public byte ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        throw new InvalidOperationException("Cannot convert null to byte.");
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        return byte.Parse((string)(object)input, CultureInfo.InvariantCulture); // Parse string to byte
                    }
                    else if (underlyingType == typeof(int))
                    {
                        return Convert.ToByte(input); // Convert int to byte
                    }
                    else if (underlyingType == typeof(double))
                    {
                        return Convert.ToByte(input); // Convert double to byte
                    }
                    else if (underlyingType == typeof(short))
                    {
                        return Convert.ToByte(input); // Convert short to byte
                    }
                    else if (underlyingType == typeof(float))
                    {
                        return Convert.ToByte(input); // Convert float to byte
                    }
                    else
                    {
                        throw new InvalidOperationException($"Conversion from {inputType.Name} to byte is not supported.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to byte.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a byte.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of byte.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out byte result)
            {
                return byte.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out result);
            }
        }
    }
}
