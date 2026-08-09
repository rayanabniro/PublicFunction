using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;


namespace PublicFunction.Converter
{
    public class DataTableField
    {
        /// <summary>
        /// Provides comprehensive DataTable field manipulation and analysis services.
        /// This interface defines methods for extracting, transforming, searching, and analyzing
        /// field values within a DataTable.
        /// </summary>
        public interface IDataTableFieldHelper
        {
            #region Field Extraction

            /// <summary>
            /// Extracts values from a specified column as an array of type T.
            /// Handles DBNull values and type conversions automatically.
            /// </summary>
            /// <typeparam name="T">The target type for the extracted values</typeparam>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnName">The name of the column to extract values from</param>
            /// <param name="defaultValue">Default value to use when conversion fails or value is null</param>
            /// <returns>An array of type T containing the extracted values</returns>
            /// <exception cref="ArgumentException">Thrown when the column does not exist in the DataTable</exception>
            T[] GetFieldValues<T>(DataTable table, string columnName, T defaultValue = default(T));

            /// <summary>
            /// Extracts values from multiple columns simultaneously.
            /// Returns a dictionary where each key is a column name and each value is an array of extracted values.
            /// </summary>
            /// <typeparam name="T">The target type for the extracted values</typeparam>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnNames">Array of column names to extract</param>
            /// <returns>Dictionary with column names as keys and value arrays as values</returns>
            Dictionary<string, T[]> GetMultipleFieldValues<T>(DataTable table, params string[] columnNames);

            /// <summary>
            /// Extracts values from a specified column as a List of type T.
            /// Convenience method for scenarios where a List is preferred over an array.
            /// </summary>
            /// <typeparam name="T">The target type for the extracted values</typeparam>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnName">The name of the column to extract values from</param>
            /// <returns>A List of type T containing the extracted values</returns>
            List<T> GetFieldValuesAsList<T>(DataTable table, string columnName);

            #endregion

            #region Column Transformation and Processing

            /// <summary>
            /// Converts a column to a new data type using a custom converter function.
            /// Creates a new column with the converted values while preserving the original column.
            /// </summary>
            /// <typeparam name="T">The target data type for the converted column</typeparam>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnName">The name of the column to convert</param>
            /// <param name="converter">Function that converts the original value to type T</param>
            /// <returns>The modified DataTable with the new converted column</returns>
            /// <exception cref="ArgumentException">Thrown when the column does not exist in the DataTable</exception>
            DataTable ConvertColumnType<T>(DataTable table, string columnName, Func<object, T> converter);

            /// <summary>
            /// Removes all columns that contain only null, empty, or DBNull values.
            /// Useful for cleaning up DataTables by eliminating redundant empty columns.
            /// </summary>
            /// <param name="table">The DataTable to clean</param>
            /// <returns>The modified DataTable with empty columns removed</returns>
            DataTable RemoveEmptyColumns(DataTable table);

            /// <summary>
            /// Applies a transformation function to all values in a specified column.
            /// Modifies the column values in place using the provided function.
            /// </summary>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnName">The name of the column to transform</param>
            /// <param name="function">Function that takes the original value and returns a new value</param>
            /// <returns>The modified DataTable with transformed column values</returns>
            /// <exception cref="ArgumentException">Thrown when the column does not exist in the DataTable</exception>
            DataTable ApplyFunctionToColumn(DataTable table, string columnName, Func<object, object> function);

            #endregion

            #region Search and Filter Operations

            /// <summary>
            /// Searches for rows containing a specific string value within a column.
            /// Supports case-sensitive and case-insensitive search options.
            /// </summary>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnName">The name of the column to search in</param>
            /// <param name="searchValue">The string value to search for</param>
            /// <param name="caseSensitive">If true, performs case-sensitive search; otherwise, case-insensitive</param>
            /// <returns>Array of DataRows that match the search criteria</returns>
            /// <exception cref="ArgumentException">Thrown when the column does not exist in the DataTable</exception>
            DataRow[] SearchInColumn(DataTable table, string columnName, string searchValue, bool caseSensitive = false);

            /// <summary>
            /// Finds rows where the column value falls within a specified numeric range.
            /// The type T must implement IComparable for comparison operations.
            /// </summary>
            /// <typeparam name="T">The type of the values to compare (must implement IComparable)</typeparam>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnName">The name of the column to check</param>
            /// <param name="minValue">The minimum value of the range (inclusive)</param>
            /// <param name="maxValue">The maximum value of the range (inclusive)</param>
            /// <returns>Array of DataRows where the column value is within the specified range</returns>
            /// <exception cref="ArgumentException">Thrown when the column does not exist in the DataTable</exception>
            DataRow[] FindRowsByRange<T>(DataTable table, string columnName, T minValue, T maxValue) where T : IComparable;

            #endregion

            #region Statistics and Analysis

            /// <summary>
            /// Calculates comprehensive statistics for a numeric column.
            /// Includes count, null count, min, max, average, sum, and median values.
            /// </summary>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnName">The name of the column to analyze</param>
            /// <returns>FieldStatistics object containing all calculated statistics</returns>
            /// <exception cref="ArgumentException">Thrown when the column does not exist or input is invalid</exception>
            FieldStatistics GetColumnStatistics(DataTable table, string columnName);

            /// <summary>
            /// Calculates the frequency of each distinct value in a column.
            /// Returns a dictionary with values as keys and their occurrence counts as values.
            /// </summary>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnName">The name of the column to analyze</param>
            /// <returns>Dictionary mapping each value to its frequency count</returns>
            /// <exception cref="ArgumentException">Thrown when the column does not exist in the DataTable</exception>
            Dictionary<object, int> GetValueFrequency(DataTable table, string columnName);

            #endregion

            #region Multi-Column Operations

            /// <summary>
            /// Combines values from multiple columns into a single new column.
            /// Values are concatenated using a specified separator string.
            /// </summary>
            /// <param name="table">The source DataTable</param>
            /// <param name="newColumnName">The name of the new column to create</param>
            /// <param name="separator">String used to separate values from different columns</param>
            /// <param name="columnNames">Array of column names to combine</param>
            /// <returns>The modified DataTable with the new combined column</returns>
            /// <exception cref="ArgumentException">Thrown when any specified column does not exist</exception>
            DataTable CombineColumns(DataTable table, string newColumnName, string separator, params string[] columnNames);

            /// <summary>
            /// Applies a custom function to values from multiple columns and creates a new column.
            /// The function receives an array of values from the specified columns.
            /// </summary>
            /// <param name="table">The source DataTable</param>
            /// <param name="newColumnName">The name of the new column to create</param>
            /// <param name="function">Function that takes an array of values and returns the result</param>
            /// <param name="columnNames">Array of column names whose values will be passed to the function</param>
            /// <returns>The modified DataTable with the new computed column</returns>
            /// <exception cref="ArgumentException">Thrown when any specified column does not exist</exception>
            DataTable ApplyFunctionToMultipleColumns(DataTable table, string newColumnName,
                Func<object[], object> function, params string[] columnNames);

            #endregion

            #region Helper Methods

            /// <summary>
            /// Creates a new DataTable containing only the specified columns.
            /// Useful for projecting a subset of columns from a larger DataTable.
            /// </summary>
            /// <param name="table">The source DataTable</param>
            /// <param name="columnNames">Array of column names to include in the new DataTable</param>
            /// <returns>A new DataTable containing only the specified columns</returns>
            DataTable SelectColumns(DataTable table, params string[] columnNames);

            #endregion
        }
        public class DataTableFieldHelper : IDataTableFieldHelper
        {
            #region Field Extraction

            /// <summary>
            /// Extracts values from a specified column as an array of type T.
            /// Handles DBNull values and type conversions automatically.
            /// </summary>
            public T[] GetFieldValues<T>(DataTable table, string columnName, T defaultValue = default(T))
            {
                if (table == null || string.IsNullOrEmpty(columnName))
                    return Array.Empty<T>();

                if (!table.Columns.Contains(columnName))
                    throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable");

                T[] result = new T[table.Rows.Count];

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    object value = table.Rows[i][columnName];
                    result[i] = ConvertValue<T>(value, defaultValue);
                }

                return result;
            }

            /// <summary>
            /// Extracts values from multiple columns simultaneously.
            /// </summary>
            public Dictionary<string, T[]> GetMultipleFieldValues<T>(DataTable table, params string[] columnNames)
            {
                var result = new Dictionary<string, T[]>();

                foreach (var columnName in columnNames)
                {
                    if (table.Columns.Contains(columnName))
                    {
                        result[columnName] = GetFieldValues<T>(table, columnName);
                    }
                }

                return result;
            }

            /// <summary>
            /// Extracts values from a specified column as a List of type T.
            /// </summary>
            public List<T> GetFieldValuesAsList<T>(DataTable table, string columnName)
            {
                return GetFieldValues<T>(table, columnName).ToList();
            }

            #endregion

            #region Column Transformation and Processing

            /// <summary>
            /// Converts a column to a new data type using a custom converter function.
            /// </summary>
            public DataTable ConvertColumnType<T>(DataTable table, string columnName, Func<object, T> converter)
            {
                if (table == null || string.IsNullOrEmpty(columnName))
                    return table;

                if (!table.Columns.Contains(columnName))
                    throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable");

                // Create a new column with the new type
                string newColumnName = $"{columnName}_Converted";
                table.Columns.Add(newColumnName, typeof(T));

                // Populate the new column with converted values
                foreach (DataRow row in table.Rows)
                {
                    object originalValue = row[columnName];
                    T convertedValue = converter(originalValue);
                    row[newColumnName] = convertedValue;
                }

                return table;
            }

            /// <summary>
            /// Removes all columns that contain only null, empty, or DBNull values.
            /// </summary>
            public DataTable RemoveEmptyColumns(DataTable table)
            {
                if (table == null || table.Columns.Count == 0)
                    return table;

                List<string> columnsToRemove = new List<string>();

                foreach (DataColumn col in table.Columns)
                {
                    bool isEmpty = true;
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[col] != DBNull.Value && row[col] != null && !string.IsNullOrEmpty(row[col].ToString()))
                        {
                            isEmpty = false;
                            break;
                        }
                    }

                    if (isEmpty)
                        columnsToRemove.Add(col.ColumnName);
                }

                foreach (string colName in columnsToRemove)
                {
                    table.Columns.Remove(colName);
                }

                return table;
            }

            /// <summary>
            /// Applies a transformation function to all values in a specified column.
            /// </summary>
            public DataTable ApplyFunctionToColumn(DataTable table, string columnName, Func<object, object> function)
            {
                if (table == null || string.IsNullOrEmpty(columnName))
                    return table;

                if (!table.Columns.Contains(columnName))
                    throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable");

                foreach (DataRow row in table.Rows)
                {
                    object originalValue = row[columnName];
                    row[columnName] = function(originalValue);
                }

                return table;
            }

            #endregion

            #region Search and Filter Operations

            /// <summary>
            /// Searches for rows containing a specific string value within a column.
            /// </summary>
            public DataRow[] SearchInColumn(DataTable table, string columnName, string searchValue, bool caseSensitive = false)
            {
                if (table == null || string.IsNullOrEmpty(columnName))
                    return Array.Empty<DataRow>();

                if (!table.Columns.Contains(columnName))
                    throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable");

                var results = new List<DataRow>();

                StringComparison comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                foreach (DataRow row in table.Rows)
                {
                    object value = row[columnName];
                    if (value != null && value != DBNull.Value)
                    {
                        string strValue = value.ToString();
                        if (strValue.IndexOf(searchValue, comparison) >= 0)
                        {
                            results.Add(row);
                        }
                    }
                }

                return results.ToArray();
            }

            /// <summary>
            /// Finds rows where the column value falls within a specified numeric range.
            /// </summary>
            public DataRow[] FindRowsByRange<T>(DataTable table, string columnName, T minValue, T maxValue) where T : IComparable
            {
                if (table == null || string.IsNullOrEmpty(columnName))
                    return Array.Empty<DataRow>();

                if (!table.Columns.Contains(columnName))
                    throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable");

                var results = new List<DataRow>();

                foreach (DataRow row in table.Rows)
                {
                    object value = row[columnName];
                    if (value != null && value != DBNull.Value)
                    {
                        try
                        {
                            T typedValue = ConvertValue<T>(value);
                            if (typedValue.CompareTo(minValue) >= 0 && typedValue.CompareTo(maxValue) <= 0)
                            {
                                results.Add(row);
                            }
                        }
                        catch
                        {
                            // Ignore conversion failures
                        }
                    }
                }

                return results.ToArray();
            }

            #endregion

            #region Statistics and Analysis

            /// <summary>
            /// Calculates comprehensive statistics for a numeric column.
            /// </summary>
            public FieldStatistics GetColumnStatistics(DataTable table, string columnName)
            {
                if (table == null || string.IsNullOrEmpty(columnName))
                    throw new ArgumentException("Invalid input parameters");

                if (!table.Columns.Contains(columnName))
                    throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable");

                var values = new List<double>();
                int nullCount = 0;

                foreach (DataRow row in table.Rows)
                {
                    object value = row[columnName];
                    if (value == null || value == DBNull.Value)
                    {
                        nullCount++;
                        continue;
                    }

                    try
                    {
                        double doubleValue = Convert.ToDouble(value);
                        values.Add(doubleValue);
                    }
                    catch
                    {
                        // Ignore non-numeric values
                    }
                }

                if (values.Count == 0)
                    return new FieldStatistics { NullCount = nullCount };

                values.Sort();

                return new FieldStatistics
                {
                    Count = values.Count,
                    NullCount = nullCount,
                    Min = values.Min(),
                    Max = values.Max(),
                    Average = values.Average(),
                    Sum = values.Sum(),
                    Median = values.Count % 2 == 0 ? (values[values.Count / 2 - 1] + values[values.Count / 2]) / 2 : values[values.Count / 2]
                };
            }

            /// <summary>
            /// Calculates the frequency of each distinct value in a column.
            /// </summary>
            public Dictionary<object, int> GetValueFrequency(DataTable table, string columnName)
            {
                if (table == null || string.IsNullOrEmpty(columnName))
                    return new Dictionary<object, int>();

                if (!table.Columns.Contains(columnName))
                    throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable");

                var frequency = new Dictionary<object, int>();

                foreach (DataRow row in table.Rows)
                {
                    object value = row[columnName];
                    if (value == DBNull.Value)
                        value = null;

                    if (frequency.ContainsKey(value))
                        frequency[value]++;
                    else
                        frequency[value] = 1;
                }

                return frequency;
            }

            #endregion

            #region Multi-Column Operations

            /// <summary>
            /// Combines values from multiple columns into a single new column.
            /// </summary>
            public DataTable CombineColumns(DataTable table, string newColumnName, string separator, params string[] columnNames)
            {
                if (table == null || columnNames.Length == 0)
                    return table;

                // Verify all columns exist
                foreach (string colName in columnNames)
                {
                    if (!table.Columns.Contains(colName))
                        throw new ArgumentException($"Column '{colName}' does not exist in the DataTable");
                }

                table.Columns.Add(newColumnName, typeof(string));

                foreach (DataRow row in table.Rows)
                {
                    var parts = new List<string>();
                    foreach (string colName in columnNames)
                    {
                        object value = row[colName];
                        parts.Add(value?.ToString() ?? string.Empty);
                    }
                    row[newColumnName] = string.Join(separator, parts);
                }

                return table;
            }

            /// <summary>
            /// Applies a custom function to values from multiple columns and creates a new column.
            /// </summary>
            public DataTable ApplyFunctionToMultipleColumns(DataTable table, string newColumnName,
                Func<object[], object> function, params string[] columnNames)
            {
                if (table == null || columnNames.Length == 0)
                    return table;

                // Verify all columns exist
                foreach (string colName in columnNames)
                {
                    if (!table.Columns.Contains(colName))
                        throw new ArgumentException($"Column '{colName}' does not exist in the DataTable");
                }

                table.Columns.Add(newColumnName, typeof(object));

                foreach (DataRow row in table.Rows)
                {
                    object[] values = new object[columnNames.Length];
                    for (int i = 0; i < columnNames.Length; i++)
                    {
                        values[i] = row[columnNames[i]];
                    }
                    row[newColumnName] = function(values);
                }

                return table;
            }

            #endregion

            #region Helper Methods

            /// <summary>
            /// Creates a new DataTable containing only the specified columns.
            /// </summary>
            public DataTable SelectColumns(DataTable table, params string[] columnNames)
            {
                if (table == null || columnNames.Length == 0)
                    return new DataTable();

                DataTable newTable = new DataTable();

                // Add specified columns
                foreach (string colName in columnNames)
                {
                    if (table.Columns.Contains(colName))
                    {
                        newTable.Columns.Add(colName, table.Columns[colName].DataType);
                    }
                }

                // Copy data
                foreach (DataRow row in table.Rows)
                {
                    DataRow newRow = newTable.NewRow();
                    foreach (DataColumn col in newTable.Columns)
                    {
                        newRow[col.ColumnName] = row[col.ColumnName];
                    }
                    newTable.Rows.Add(newRow);
                }

                return newTable;
            }

            /// <summary>
            /// Converts a value to type T with error handling.
            /// </summary>
            private T ConvertValue<T>(object value, T defaultValue = default(T))
            {
                if (value == null || value == DBNull.Value)
                    return defaultValue;

                try
                {
                    if (value.GetType() == typeof(T))
                        return (T)value;

                    // Handle Nullable types
                    Type targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

                    if (targetType.IsEnum)
                    {
                        return (T)Enum.ToObject(targetType, value);
                    }

                    return (T)Convert.ChangeType(value, targetType);
                }
                catch
                {
                    return defaultValue;
                }
            }

            #endregion
        }

        /// <summary>
        /// Represents statistical information calculated for a DataTable column.
        /// Contains comprehensive metrics including count, min, max, average, sum, and median.
        /// </summary>
        public class FieldStatistics
        {
            /// <summary>Total number of valid (non-null) values</summary>
            public int Count { get; set; }

            /// <summary>Number of null or DBNull values in the column</summary>
            public int NullCount { get; set; }

            /// <summary>Minimum value in the column</summary>
            public double Min { get; set; }

            /// <summary>Maximum value in the column</summary>
            public double Max { get; set; }

            /// <summary>Average (mean) value of the column</summary>
            public double Average { get; set; }

            /// <summary>Sum of all values in the column</summary>
            public double Sum { get; set; }

            /// <summary>Median value of the column (middle value when sorted)</summary>
            public double Median { get; set; }

            /// <summary>
            /// Returns a string representation of the statistics.
            /// </summary>
            public override string ToString()
            {
                return $"Count: {Count}, Average: {Average:F2}, Min: {Min}, Max: {Max}, Sum: {Sum}, Median: {Median:F2}, Null Count: {NullCount}";
            }
        }
    }
}