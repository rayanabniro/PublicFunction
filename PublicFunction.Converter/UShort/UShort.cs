using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter.UShort
{
    public class UShort
    {
        public interface IUShort
        {
            T ConvertTo<T>(ushort input);
            ushort ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out ushort result);
        }

        public class UShortService : IUShort
        {
            /// <summary>
            /// Converts a ushort input to the specified type T.
            /// Supports conversions to string, int, long, etc.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The ushort input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(ushort input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // Handle conversions to common types
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString(); // Convert ushort to string
                    }
                    else if (targetType == typeof(int))
                    {
                        return (T)(object)(int)input; // Convert ushort to int
                    }
                    else if (targetType == typeof(long))
                    {
                        return (T)(object)(long)input; // Convert ushort to long
                    }
                    else if (targetType == typeof(double))
                    {
                        return (T)(object)(double)input; // Convert ushort to double
                    }
                    else if (targetType == typeof(float))
                    {
                        return (T)(object)(float)input; // Convert ushort to float
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
            /// Converts an object of type T to its ushort representation.
            /// Supports conversions from string, int, long, etc.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to ushort.</param>
            /// <returns>The ushort representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public ushort ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        throw new InvalidOperationException("Cannot convert null to ushort.");
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        return ushort.Parse((string)(object)input, CultureInfo.InvariantCulture); // Parse string to ushort
                    }
                    else if (underlyingType == typeof(int))
                    {
                        int intValue = Convert.ToInt32(input);
                        if (intValue < 0 || intValue > ushort.MaxValue)
                        {
                            throw new InvalidOperationException($"Cannot convert {intValue} to ushort as it is out of range.");
                        }
                        return (ushort)intValue; // Convert int to ushort
                    }
                    else if (underlyingType == typeof(long))
                    {
                        long longValue = Convert.ToInt64(input);
                        if (longValue < 0 || longValue > ushort.MaxValue)
                        {
                            throw new InvalidOperationException($"Cannot convert {longValue} to ushort as it is out of range.");
                        }
                        return (ushort)longValue; // Convert long to ushort
                    }
                    else if (underlyingType == typeof(double))
                    {
                        return (ushort)Convert.ToDouble(input); // Convert double to ushort
                    }
                    else if (underlyingType == typeof(float))
                    {
                        return (ushort)Convert.ToSingle(input); // Convert float to ushort
                    }
                    else
                    {
                        throw new InvalidOperationException($"Conversion from {inputType.Name} to ushort is not supported.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to ushort.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a ushort.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of ushort.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out ushort result)
            {
                return ushort.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out result);
            }
        }
    }
}
