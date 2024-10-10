using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter
{
    public class Bool
    {
        public interface IBool
        {
            T ConvertTo<T>(bool input);
            bool ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out bool result);
        }

        public class BoolService : IBool
        {
            /// <summary>
            /// Converts a bool input to the specified type T.
            /// Supports conversions to string, int, double, and others.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The bool input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(bool input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // If the target type is string, return "true" or "false"
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString().ToLower(); // Convert to lowercase "true" or "false"
                    }
                    else if (targetType == typeof(int))
                    {
                        return (T)(object)(input ? 1 : 0); // Convert to 1 or 0
                    }
                    else if (targetType == typeof(double))
                    {
                        return (T)(object)(input ? 1.0 : 0.0); // Convert to 1.0 or 0.0
                    }
                    else if (targetType.IsEnum)
                    {
                        return (T)Enum.ToObject(targetType, input ? 1 : 0); // Convert to enum based on bool
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
            /// Converts an object of type T to its bool representation.
            /// Supports string, int, double, and other types.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to bool.</param>
            /// <returns>The bool representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public bool ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        return false;
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        return bool.TryParse((string)(object)input, out bool result) && result;
                    }
                    else if (underlyingType == typeof(int))
                    {
                        return ((int)(object)input) != 0; // Convert 0 to false, any other int to true
                    }
                    else if (underlyingType == typeof(double))
                    {
                        return Math.Abs((double)(object)input) > 0.0; // Convert non-zero double to true
                    }
                    else
                    {
                        return Convert.ToBoolean(input, CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to bool.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a bool.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, false.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out bool result)
            {
                return bool.TryParse(input, out result);
            }
        }
    }
}
