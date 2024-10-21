using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter
{
    public class Float
    {
        public interface IFloat
        {
            T ConvertTo<T>(float input);
            float ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out float result);
        }

        public class FloatService : IFloat
        {
            /// <summary>
            /// Converts a float input to the specified type T.
            /// Supports conversions to string, int, double, bool, and others.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The float input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(float input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // If the target type is string, return the float as a string
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
                        return (T)(object)(input != 0); // Convert non-zero float to true, zero to false
                    }
                    else if (targetType.IsEnum)
                    {
                        return (T)Enum.ToObject(targetType, input); // Convert to enum based on float
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
            /// Converts an object of type T to its float representation.
            /// Supports string, int, double, bool, and other types.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to float.</param>
            /// <returns>The float representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public float ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        return 0f; // Default value for float
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        return float.Parse((string)(object)input, CultureInfo.InvariantCulture); // Parse string to float
                    }
                    else if (underlyingType == typeof(int))
                    {
                        return Convert.ToSingle((int)(object)input); // Convert int to float
                    }
                    else if (underlyingType == typeof(double))
                    {
                        return Convert.ToSingle((double)(object)input); // Convert double to float
                    }
                    else if (underlyingType == typeof(bool))
                    {
                        return (bool)(object)input ? 1f : 0f; // Convert bool to float (true to 1.0f, false to 0.0f)
                    }
                    else
                    {
                        return Convert.ToSingle(input, CultureInfo.InvariantCulture); // Fallback to Convert
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to float.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a float.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of float.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out float result)
            {
                return float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
            }
        }
    }
}
