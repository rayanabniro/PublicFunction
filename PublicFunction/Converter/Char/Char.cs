using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Converter.Char
{
    public class Char
    {
        public interface IChar
        {
            T ConvertTo<T>(char input);
            char ConvertFrom<T>(T input);
            bool TryConvertTo(string input, out char result);
        }

        public class CharService : IChar
        {
            /// <summary>
            /// Converts a char input to the specified type T.
            /// Supports conversions to string, int, etc.
            /// </summary>
            /// <typeparam name="T">The target type to convert to.</typeparam>
            /// <param name="input">The char input to convert.</param>
            /// <returns>The converted value of type T.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public T ConvertTo<T>(char input)
            {
                try
                {
                    Type targetType = typeof(T);

                    // Handle conversions to common types
                    if (targetType == typeof(string))
                    {
                        return (T)(object)input.ToString(); // Convert char to string
                    }
                    else if (targetType == typeof(int))
                    {
                        return (T)(object)(int)input; // Convert char to int (ASCII value)
                    }
                    else if (targetType == typeof(byte))
                    {
                        return (T)(object)(byte)input; // Convert char to byte
                    }
                    else if (targetType == typeof(bool))
                    {
                        return (T)(object)(input != '\0'); // Convert char to bool (non-null char)
                    }
                    else
                    {
                        throw new InvalidOperationException($"Conversion from char to {targetType.Name} is not supported.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert {input} to {typeof(T).Name}.", ex);
                }
            }

            /// <summary>
            /// Converts an object of type T to its char representation.
            /// Supports conversions from string, int, etc.
            /// </summary>
            /// <typeparam name="T">The type of the input object.</typeparam>
            /// <param name="input">The object to convert to char.</param>
            /// <returns>The char representation of the input object.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the conversion fails or the type is not supported.
            /// </exception>
            public char ConvertFrom<T>(T input)
            {
                try
                {
                    if (input == null)
                    {
                        throw new InvalidOperationException("Cannot convert null to char.");
                    }

                    Type inputType = typeof(T);
                    Type underlyingType = Nullable.GetUnderlyingType(inputType) ?? inputType;

                    if (underlyingType == typeof(string))
                    {
                        string str = (string)(object)input;
                        if (str.Length == 0)
                        {
                            throw new InvalidOperationException("Cannot convert empty string to char.");
                        }
                        return str[0]; // Convert string to char (first character)
                    }
                    else if (underlyingType == typeof(int))
                    {
                        int intValue = Convert.ToInt32(input);
                        if (intValue < char.MinValue || intValue > char.MaxValue)
                        {
                            throw new InvalidOperationException($"Cannot convert {intValue} to char as it is out of range.");
                        }
                        return (char)intValue; // Convert int to char
                    }
                    else if (underlyingType == typeof(byte))
                    {
                        return (char)Convert.ToByte(input); // Convert byte to char
                    }
                    else
                    {
                        throw new InvalidOperationException($"Conversion from {inputType.Name} to char is not supported.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert from {typeof(T).Name} to char.", ex);
                }
            }

            /// <summary>
            /// Attempts to convert a string input to a char.
            /// Returns a boolean indicating success or failure.
            /// </summary>
            /// <param name="input">The string input to convert.</param>
            /// <param name="result">The converted value if successful; otherwise, the default value of char.</param>
            /// <returns>True if conversion is successful; otherwise, false.</returns>
            public bool TryConvertTo(string input, out char result)
            {
                if (string.IsNullOrEmpty(input) || input.Length > 1)
                {
                    result = default;
                    return false; // Cannot convert if input is empty or more than one character
                }

                result = input[0]; // Convert single character string to char
                return true;
            }
        }
    }
}
