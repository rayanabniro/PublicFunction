```markdown
# DataTableField Helper Documentation

## Overview

The `DataTableField` class provides a comprehensive set of utilities for working with DataTable fields in C#. It offers methods for extracting, transforming, searching, analyzing, and manipulating column data with type safety and error handling.

**Namespace:** `PublicFunction.Converter`

**Class Hierarchy:**
- `DataTableField` (Container Class)
  - `IDataTableFieldHelper` (Interface)
  - `DataTableFieldHelper` (Implementation)
  - `FieldStatistics` (Data Model)

---

## Table of Contents

1. [Installation](#installation)
2. [Quick Start](#quick-start)
3. [API Reference](#api-reference)
   - [Field Extraction](#field-extraction)
   - [Column Transformation](#column-transformation)
   - [Search and Filter](#search-and-filter)
   - [Statistics and Analysis](#statistics-and-analysis)
   - [Multi-Column Operations](#multi-column-operations)
   - [Helper Methods](#helper-methods)
4. [Advanced Examples](#advanced-examples)
5. [Error Handling](#error-handling)
6. [Performance Considerations](#performance-considerations)
7. [Best Practices](#best-practices)

---

## Installation

### Using NuGet (If Available)

```bash
dotnet add package PublicFunction.Converter
```

### Manual Inclusion

1. Copy the `DataTableField.cs` file to your project
2. Add the required using statements:

```csharp
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
```

---

## Quick Start

```csharp
using PublicFunction.Converter;

// Create an instance
DataTableField.IDataTableFieldHelper helper = new DataTableField.DataTableFieldHelper();

// Create a sample DataTable
DataTable table = new DataTable();
table.Columns.Add("Id", typeof(int));
table.Columns.Add("Name", typeof(string));
table.Columns.Add("Price", typeof(decimal));
table.Rows.Add(1, "Product A", 99.99);
table.Rows.Add(2, "Product B", 149.50);

// Extract values
int[] ids = helper.GetFieldValues<int>(table, "Id");
string[] names = helper.GetFieldValues<string>(table, "Name", "Unknown");
decimal[] prices = helper.GetFieldValues<decimal>(table, "Price", 0);

// Get statistics
FieldStatistics stats = helper.GetColumnStatistics(table, "Price");
Console.WriteLine(stats);
```

---

## API Reference

### Field Extraction

#### `GetFieldValues<T>`

Extracts values from a specified column as an array of type T.

```csharp
T[] GetFieldValues<T>(DataTable table, string columnName, T defaultValue = default(T))
```

**Parameters:**
- `table`: Source DataTable
- `columnName`: Name of the column to extract
- `defaultValue`: Default value for null or conversion errors

**Returns:** Array of type T

**Example:**
```csharp
int[] ids = helper.GetFieldValues<int>(table, "Id");
string[] names = helper.GetFieldValues<string>(table, "Name", "N/A");
DateTime[] dates = helper.GetFieldValues<DateTime>(table, "CreatedDate", DateTime.Now);
```

---

#### `GetMultipleFieldValues<T>`

Extracts values from multiple columns simultaneously.

```csharp
Dictionary<string, T[]> GetMultipleFieldValues<T>(DataTable table, params string[] columnNames)
```

**Parameters:**
- `table`: Source DataTable
- `columnNames`: Array of column names to extract

**Returns:** Dictionary with column names as keys and value arrays as values

**Example:**
```csharp
var extracted = helper.GetMultipleFieldValues<string>(table, "Name", "Category", "Description");
foreach (var kvp in extracted)
{
    Console.WriteLine($"Column: {kvp.Key}, Values: {string.Join(", ", kvp.Value)}");
}
```

---

#### `GetFieldValuesAsList<T>`

Extracts values as a List of type T.

```csharp
List<T> GetFieldValuesAsList<T>(DataTable table, string columnName)
```

**Parameters:**
- `table`: Source DataTable
- `columnName`: Name of the column to extract

**Returns:** List of type T

**Example:**
```csharp
List<string> names = helper.GetFieldValuesAsList<string>(table, "Name");
names.ForEach(name => Console.WriteLine(name));
```

---

### Column Transformation

#### `ConvertColumnType<T>`

Converts a column to a new data type using a custom converter function.

```csharp
DataTable ConvertColumnType<T>(DataTable table, string columnName, Func<object, T> converter)
```

**Parameters:**
- `table`: Source DataTable
- `columnName`: Name of the column to convert
- `converter`: Function that converts the original value to type T

**Returns:** Modified DataTable with the new converted column

**Example:**
```csharp
// Convert string date to DateTime
helper.ConvertColumnType<DateTime>(table, "DateString", 
    val => DateTime.Parse(val.ToString()));

// Convert string to int with error handling
helper.ConvertColumnType<int>(table, "AgeString", 
    val => int.TryParse(val.ToString(), out int result) ? result : 0);
```

---

#### `RemoveEmptyColumns`

Removes all columns that contain only null, empty, or DBNull values.

```csharp
DataTable RemoveEmptyColumns(DataTable table)
```

**Parameters:**
- `table`: DataTable to clean

**Returns:** Modified DataTable with empty columns removed

**Example:**
```csharp
helper.RemoveEmptyColumns(table);
// All columns with only null/empty values are now removed
```

---

#### `ApplyFunctionToColumn`

Applies a transformation function to all values in a specified column.

```csharp
DataTable ApplyFunctionToColumn(DataTable table, string columnName, Func<object, object> function)
```

**Parameters:**
- `table`: Source DataTable
- `columnName`: Name of the column to transform
- `function`: Function that takes the original value and returns a new value

**Returns:** Modified DataTable with transformed column values

**Example:**
```csharp
// Increase all prices by 10%
helper.ApplyFunctionToColumn(table, "Price", 
    val => Convert.ToDecimal(val) * 1.1m);

// Convert all names to uppercase
helper.ApplyFunctionToColumn(table, "Name", 
    val => val?.ToString().ToUpper());

// Clean string values
helper.ApplyFunctionToColumn(table, "Description", 
    val => val?.ToString().Trim());
```

---

### Search and Filter

#### `SearchInColumn`

Searches for rows containing a specific string value within a column.

```csharp
DataRow[] SearchInColumn(DataTable table, string columnName, string searchValue, bool caseSensitive = false)
```

**Parameters:**
- `table`: Source DataTable
- `columnName`: Name of the column to search
- `searchValue`: String value to search for
- `caseSensitive`: Whether the search is case-sensitive

**Returns:** Array of matching DataRows

**Example:**
```csharp
// Case-insensitive search
DataRow[] results = helper.SearchInColumn(table, "Name", "Product");

// Case-sensitive search
DataRow[] exactResults = helper.SearchInColumn(table, "Code", "ABC123", true);

foreach (DataRow row in results)
{
    Console.WriteLine($"Found: {row["Name"]}");
}
```

---

#### `FindRowsByRange`

Finds rows where the column value falls within a specified numeric range.

```csharp
DataRow[] FindRowsByRange<T>(DataTable table, string columnName, T minValue, T maxValue) where T : IComparable
```

**Parameters:**
- `table`: Source DataTable
- `columnName`: Name of the column to check
- `minValue`: Minimum value (inclusive)
- `maxValue`: Maximum value (inclusive)

**Returns:** Array of matching DataRows

**Example:**
```csharp
// Find products with price between 100 and 200
DataRow[] products = helper.FindRowsByRange<decimal>(table, "Price", 100m, 200m);

// Find dates in a specific range
DataRow[] dateRows = helper.FindRowsByRange<DateTime>(table, "Date", 
    new DateTime(2024, 1, 1), new DateTime(2024, 12, 31));

// Find ages between 18 and 65
DataRow[] adults = helper.FindRowsByRange<int>(table, "Age", 18, 65);
```

---

### Statistics and Analysis

#### `GetColumnStatistics`

Calculates comprehensive statistics for a numeric column.

```csharp
FieldStatistics GetColumnStatistics(DataTable table, string columnName)
```

**Parameters:**
- `table`: Source DataTable
- `columnName`: Name of the column to analyze

**Returns:** `FieldStatistics` object containing all calculated metrics

**Example:**
```csharp
FieldStatistics stats = helper.GetColumnStatistics(table, "Price");
Console.WriteLine($"Count: {stats.Count}");
Console.WriteLine($"Average: {stats.Average:F2}");
Console.WriteLine($"Min: {stats.Min:F2}");
Console.WriteLine($"Max: {stats.Max:F2}");
Console.WriteLine($"Sum: {stats.Sum:F2}");
Console.WriteLine($"Median: {stats.Median:F2}");
Console.WriteLine($"Null Count: {stats.NullCount}");
```

**FieldStatistics Properties:**
- `Count`: Total number of valid (non-null) values
- `NullCount`: Number of null or DBNull values
- `Min`: Minimum value
- `Max`: Maximum value
- `Average`: Mean value
- `Sum`: Sum of all values
- `Median`: Middle value when sorted

---

#### `GetValueFrequency`

Calculates the frequency of each distinct value in a column.

```csharp
Dictionary<object, int> GetValueFrequency(DataTable table, string columnName)
```

**Parameters:**
- `table`: Source DataTable
- `columnName`: Name of the column to analyze

**Returns:** Dictionary mapping each value to its frequency count

**Example:**
```csharp
var frequency = helper.GetValueFrequency(table, "Category");
foreach (var kvp in frequency)
{
    Console.WriteLine($"Category: {kvp.Key ?? "NULL"}, Count: {kvp.Value}");
}

// Find the most common value
var mostCommon = frequency.OrderByDescending(x => x.Value).First();
Console.WriteLine($"Most common: {mostCommon.Key} ({mostCommon.Value} times)");
```

---

### Multi-Column Operations

#### `CombineColumns`

Combines values from multiple columns into a single new column.

```csharp
DataTable CombineColumns(DataTable table, string newColumnName, string separator, params string[] columnNames)
```

**Parameters:**
- `table`: Source DataTable
- `newColumnName`: Name of the new column to create
- `separator`: String used to separate values
- `columnNames`: Array of column names to combine

**Returns:** Modified DataTable with the new combined column

**Example:**
```csharp
// Combine first name and last name
helper.CombineColumns(table, "FullName", " ", "FirstName", "LastName");

// Create a description from multiple columns
helper.CombineColumns(table, "ProductInfo", " | ", "Id", "Name", "Price");

// Combine with custom format
helper.CombineColumns(table, "Address", ", ", "Street", "City", "Country");
```

---

#### `ApplyFunctionToMultipleColumns`

Applies a custom function to values from multiple columns and creates a new column.

```csharp
DataTable ApplyFunctionToMultipleColumns(DataTable table, string newColumnName, 
    Func<object[], object> function, params string[] columnNames)
```

**Parameters:**
- `table`: Source DataTable
- `newColumnName`: Name of the new column to create
- `function`: Function that takes an array of values and returns the result
- `columnNames`: Array of column names to process

**Returns:** Modified DataTable with the new computed column

**Example:**
```csharp
// Calculate total price (Price * Quantity)
helper.ApplyFunctionToMultipleColumns(table, "TotalPrice",
    values => Convert.ToDecimal(values[0]) * Convert.ToInt32(values[1]),
    "Price", "Quantity");

// Calculate average of multiple scores
helper.ApplyFunctionToMultipleColumns(table, "AverageScore",
    values => values.Select(v => Convert.ToDouble(v)).Average(),
    "Score1", "Score2", "Score3");

// Create a formatted string from multiple columns
helper.ApplyFunctionToMultipleColumns(table, "FullDescription",
    values => $"{values[0]} - {values[1]} ({values[2]:C})",
    "Id", "Name", "Price");
```

---

### Helper Methods

#### `SelectColumns`

Creates a new DataTable containing only the specified columns.

```csharp
DataTable SelectColumns(DataTable table, params string[] columnNames)
```

**Parameters:**
- `table`: Source DataTable
- `columnNames`: Array of column names to include

**Returns:** A new DataTable containing only the specified columns

**Example:**
```csharp
// Select only essential columns
DataTable minimalTable = helper.SelectColumns(table, "Id", "Name", "Price");

// Export specific columns for reporting
DataTable reportData = helper.SelectColumns(table, "ProductName", "Category", "Sales");
```

---

## Advanced Examples

### Complex Data Processing

```csharp
// Create a comprehensive data processing pipeline
public DataTable ProcessSalesData(DataTable rawData)
{
    var helper = new DataTableField.DataTableFieldHelper();
    
    // 1. Clean the data
    helper.RemoveEmptyColumns(rawData);
    
    // 2. Convert string dates to DateTime
    helper.ConvertColumnType<DateTime>(rawData, "DateString", 
        val => DateTime.Parse(val.ToString()));
    
    // 3. Calculate total revenue
    helper.ApplyFunctionToMultipleColumns(rawData, "Revenue",
        values => Convert.ToDecimal(values[0]) * Convert.ToInt32(values[1]),
        "UnitPrice", "Quantity");
    
    // 4. Apply discount to prices
    helper.ApplyFunctionToColumn(rawData, "UnitPrice",
        val => Convert.ToDecimal(val) * 0.9m);
    
    // 5. Add a status column
    helper.ApplyFunctionToMultipleColumns(rawData, "Status",
        values => Convert.ToDecimal(values[0]) > 500 ? "High Value" : "Standard",
        "Revenue");
    
    // 6. Get statistics for reporting
    var stats = helper.GetColumnStatistics(rawData, "Revenue");
    Console.WriteLine($"Total Revenue: {stats.Sum:C}");
    Console.WriteLine($"Average Revenue: {stats.Average:C}");
    Console.WriteLine($"Revenue Range: {stats.Min:C} - {stats.Max:C}");
    
    // 7. Find high-value orders
    var highValueOrders = helper.FindRowsByRange<decimal>(rawData, "Revenue", 1000m, decimal.MaxValue);
    Console.WriteLine($"High-value orders: {highValueOrders.Length}");
    
    return rawData;
}
```

### Reporting Example

```csharp
public string GenerateReport(DataTable data)
{
    var helper = new DataTableField.DataTableFieldHelper();
    var report = new StringBuilder();
    
    // Category analysis
    var categoryFrequency = helper.GetValueFrequency(data, "Category");
    report.AppendLine("=== Category Report ===");
    foreach (var kvp in categoryFrequency)
    {
        report.AppendLine($"  {kvp.Key}: {kvp.Value} items");
    }
    
    // Revenue statistics by category
    var categories = data.AsEnumerable()
        .Select(row => row["Category"].ToString())
        .Distinct();
    
    foreach (var category in categories)
    {
        var categoryData = data.AsEnumerable()
            .Where(row => row["Category"].ToString() == category)
            .CopyToDataTable();
            
        var stats = helper.GetColumnStatistics(categoryData, "Revenue");
        report.AppendLine($"\n=== {category} Statistics ===");
        report.AppendLine($"  Total: {stats.Sum:C}");
        report.AppendLine($"  Average: {stats.Average:C}");
        report.AppendLine($"  Count: {stats.Count}");
        report.AppendLine($"  Range: {stats.Min:C} - {stats.Max:C}");
    }
    
    return report.ToString();
}
```

---

## Error Handling

The library implements comprehensive error handling:

```csharp
try
{
    var values = helper.GetFieldValues<int>(table, "NonExistentColumn");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Column error: {ex.Message}");
}

try
{
    var stats = helper.GetColumnStatistics(table, "StringColumn");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Statistics error: {ex.Message}");
}

// Safe extraction with default values
int[] safeValues = helper.GetFieldValues<int>(table, "Column", 0);
// Any conversion errors will result in the default value
```

---

## Performance Considerations

1. **Large DataTables**: Use `GetFieldValues` instead of LINQ for better performance
2. **Multiple Columns**: Use `GetMultipleFieldValues` to avoid multiple traversals
3. **Statistics**: The `GetColumnStatistics` method sorts values internally (O(n log n))
4. **Frequency Analysis**: Uses Dictionary for O(1) lookups
5. **Memory Usage**: Methods create new arrays or modify existing DataTables in-place

**Performance Tips:**
```csharp
// Efficient: Single traversal
var multipleColumns = helper.GetMultipleFieldValues<string>(table, "Col1", "Col2", "Col3");

// Less efficient: Multiple traversals
var col1 = helper.GetFieldValues<string>(table, "Col1");
var col2 = helper.GetFieldValues<string>(table, "Col2");
var col3 = helper.GetFieldValues<string>(table, "Col3");
```

---

## Best Practices

### 1. Validate Input
```csharp
if (table == null || table.Rows.Count == 0)
    return new List<string>();
```

### 2. Use Default Values
```csharp
// Provide meaningful defaults
var names = helper.GetFieldValues<string>(table, "Name", "Unknown");
var prices = helper.GetFieldValues<decimal>(table, "Price", 0m);
```

### 3. Handle Exceptions
```csharp
try
{
    var stats = helper.GetColumnStatistics(table, "Revenue");
    ProcessStatistics(stats);
}
catch (ArgumentException ex)
{
    Logger.LogError($"Column analysis failed: {ex.Message}");
    return default;
}
```

### 4. Cache Results
```csharp
// Cache frequently accessed data
private Dictionary<string, object[]> _cachedValues;

public object[] GetCachedFieldValues(DataTable table, string columnName)
{
    if (!_cachedValues.ContainsKey(columnName))
    {
        _cachedValues[columnName] = helper.GetFieldValues<object>(table, columnName);
    }
    return _cachedValues[columnName];
}
```

### 5. Use Type-Safe Methods
```csharp
// Good: Type-safe
int[] ids = helper.GetFieldValues<int>(table, "Id");

// Bad: Using object when a specific type is known
object[] ids = helper.GetFieldValues<object>(table, "Id");
```

### 6. Combine Operations
```csharp
// Efficient: Chain operations
helper.ApplyFunctionToColumn(table, "Price", val => Convert.ToDecimal(val) * 1.1m);
helper.CombineColumns(table, "FullInfo", " | ", "Id", "Name", "Price");
var stats = helper.GetColumnStatistics(table, "Price");
```

---

## Common Use Cases

### Data Export
```csharp
public string ExportToCsv(DataTable data)
{
    var helper = new DataTableField.DataTableFieldHelper();
    var csv = new StringBuilder();
    
    // Select only needed columns
    var exportData = helper.SelectColumns(data, "Id", "Name", "Price", "Quantity");
    
    // Calculate total
    var stats = helper.GetColumnStatistics(exportData, "Price");
    csv.AppendLine($"Total Products: {stats.Count}");
    csv.AppendLine($"Average Price: {stats.Average:C}");
    
    return csv.ToString();
}
```

### Data Validation
```csharp
public ValidationResult ValidateData(DataTable data)
{
    var helper = new DataTableField.DataTableFieldHelper();
    var result = new ValidationResult();
    
    // Check for empty values
    foreach (DataColumn col in data.Columns)
    {
        var values = helper.GetFieldValues<object>(data, col.ColumnName);
        if (values.Count(v => v != null) == 0)
        {
            result.Warnings.Add($"Column '{col.ColumnName}' is completely empty");
        }
    }
    
    // Check for duplicates
    var frequency = helper.GetValueFrequency(data, "Id");
    var duplicates = frequency.Where(kvp => kvp.Value > 1);
    foreach (var dup in duplicates)
    {
        result.Errors.Add($"Duplicate ID: {dup.Key} appears {dup.Value} times");
    }
    
    return result;
}
```

---

## Support

For issues or questions, please contact the development team or create an issue in the repository.

---

## Version History

- **v1.0.0**: Initial release
  - Core field extraction methods
  - Column transformation
  - Search and filter operations
  - Statistics and analysis
  - Multi-column operations

- **v1.1.0** (Upcoming):
  - Async support
  - Additional statistical functions
  - LINQ integration enhancements

