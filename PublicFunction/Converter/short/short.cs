using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter
{
    public class short
{
        public interface IShort
    {
        T ConvertTo<T>(short input);
        short ConvertFrom<T>(T input);
        bool TryConvertTo(string input, out short result);
    }

    public class ShortService : IShort
    {
        /// <summary>
        /// Converts a short input to the specified type T.
        /// Supports conversions to int, string, double, etc.
        /// </summary>
        /// <typeparam name="T">The target type to convert to.</typeparam>
        /// <param name="input">The short input to convert.</param>
        /// <returns>The converted value of type T.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the conversion fails or the type is not supported.
        /// </exception>
        public T ConvertTo<T>(short input)
        {
            try
            {
                Type targetType = typeof(T);

                // Handle conversions to common types
                if (targetType == typeof(string))
                {
                    return (T)(object)input.ToString(); // Convert short to string
                }
                else if (targetType == typeof(int))
                {
                    return (T)(object)(int)input; // Convert short to int
                }
                else if (targetType == typeof(double))
                {
                    return (T)(object)(double)input; // Convert short to double
                }
                else if (targetType == typeof(byte))
                {
                    return (T)(object)(byte)input; // Convert short to byte
                }
                else if (targetType == typeof(float))
                {
                    return (T)(object)(float)input; // Convert short to float
                }
                else if (targetType == typeof(long))
                {
                    return (T)(object)(long)input; // Convert short to long
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
        /// Converts an object of type T to its short representation.
        /// Supports conversions from string, int, etc.
        /// </summary>
        /// <typeparam name="T">The type of the input object.</typeparam>
        /// <param name="input">The object to convert to short.</param>
        /// <returns>The short representation of the input object.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the conversion fails or the type is not supported.
        /// </exception>
        public short ConvertFrom<T>(T input)
        {
            try
            {
                if (input == null)
                {
                    throw new InvalidOperationException("Cannot convert null to short.");
                }

                Type inputType = typeof(T);
                Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                if (underlyingType == typeof(string))
                {
                    return short.Parse((string)(object)input, CultureInfo.InvariantCulture); // Parse string to short
                }
                else if (underlyingType == typeof(int))
                {
                    return Convert.ToInt16(input); // Convert int to short
                }
                else if (underlyingType == typeof(double))
                {
                    return Convert.ToInt16(input); // Convert double to short
                }
                else if (underlyingType == typeof(byte))
                {
                    return Convert.ToInt16(input); // Convert byte to short
                }
                else if (underlyingType == typeof(float))
                {
                    return Convert.ToInt16(input); // Convert float to short
                }
                else
                {
                    throw new InvalidOperationException($"Conversion from {inputType.Name} to short is not supported.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to short.", ex);
            }
        }

        /// <summary>
        /// Attempts to convert a string input to a short.
        /// Returns a boolean indicating success or failure.
        /// </summary>
        /// <param name="input">The string input to convert.</param>
        /// <param name="result">The converted value if successful; otherwise, the default value of short.</param>
        /// <returns>True if conversion is successful; otherwise, false.</returns>
        public bool TryConvertTo(string input, out short result)
        {
            return short.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out result);
        }
    }
}
}
