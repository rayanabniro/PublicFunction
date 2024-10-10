using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter.Guid
{
    public class Guid
    {
        using System;
using System.Globalization;

namespace Converter
    {
        public interface IGuid
        {
            T ConvertTo<T>(Guid input);
            Guid ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out Guid result);
        }

        public class GuidService : IGuid
        {
            /// <summary>
            /// Converts a Guid input to the specified type T.
            /// Supports conversions to string, byte array, and other types.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The Guid input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(Guid input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // If the target type is string, return the Guid as a string
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString(); // Convert to string
                    }
                    else if (targetType == typeof(byte[]))
                    {
                        return (T)(object)input.ToByteArray(); // Convert to byte array
                    }
                    else if (targetType == typeof(Guid))
                    {
                        return (T)(object)input; // Return the Guid itself
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
            /// Converts an object of type T to its Guid representation.
            /// Supports string and byte array conversions.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to Guid.</param>
            /// <returns>The Guid representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public Guid ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        return Guid.Empty; // Default value for Guid
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        return Guid.Parse((string)(object)input); // Parse string to Guid
                    }
                    else if (underlyingType == typeof(byte[]))
                    {
                        return new Guid((byte[])(object)input); // Convert byte array to Guid
                    }
                    else
                    {
                        throw new InvalidOperationException($"Conversion from {inputType.Name} to Guid is not supported.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to Guid.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a Guid.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of Guid.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out Guid result)
            {
                return Guid.TryParse(input, out result);
            }
        }
    }

}
}
