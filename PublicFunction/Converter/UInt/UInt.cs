using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter.UInt
{
    public class UInt
    {
        public interface IUInt
        {
            T ConvertTo<T>(uint input);
            uint ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out uint result);
        }

        public class UIntService : IUInt
        {
            /// <summary>
            /// Converts a uint input to the specified type T.
            /// Supports conversions to string, int, long, double, etc.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The uint input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(uint input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // Handle conversions to common types
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString(); // Convert uint to string
                    }
                    else if (targetType == typeof(int))
                    {
                        if (input > int.MaxValue)
                        {
                            throw new InvalidOperationException($"Cannot convert {input} to int as it exceeds the maximum value.");
                        }
                        return (T)(object)(int)input; // Convert uint to int
                    }
                    else if (targetType == typeof(long))
                    {
                        return (T)(object)(long)input; // Convert uint to long
                    }
                    else if (targetType == typeof(double))
                    {
                        return (T)(object)(double)input; // Convert uint to double
                    }
                    else if (targetType == typeof(float))
                    {
                        return (T)(object)(float)input; // Convert uint to float
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
            /// Converts an object of type T to its uint representation.
            /// Supports conversions from string, int, long, etc.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to uint.</param>
            /// <returns>The uint representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public uint ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        throw new InvalidOperationException("Cannot convert null to uint.");
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        return uint.Parse((string)(object)input, CultureInfo.InvariantCulture); // Parse string to uint
                    }
                    else if (underlyingType == typeof(int))
                    {
                        return Convert.ToUInt32(input); // Convert int to uint
                    }
                    else if (underlyingType == typeof(long))
                    {
                        return Convert.ToUInt32(input); // Convert long to uint
                    }
                    else if (underlyingType == typeof(double))
                    {
                        return Convert.ToUInt32(input); // Convert double to uint
                    }
                    else if (underlyingType == typeof(float))
                    {
                        return Convert.ToUInt32(input); // Convert float to uint
                    }
                    else
                    {
                        throw new InvalidOperationException($"Conversion from {inputType.Name} to uint is not supported.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to uint.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a uint.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of uint.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out uint result)
            {
                return uint.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out result);
            }
        }
    }
}
