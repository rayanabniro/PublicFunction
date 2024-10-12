# PublicFunction.Converter

In this **class**, we have infrastructures that can **centrally control** For Converter 

## Use in .netCore
- in **Program.cs**  Add This dependence
  ```C#
    builder.Services.AddScoped<PublicFunction.Converter.IDatatableConvertor, PublicFunction.Converter.DatatableConvertor>();
  ```
- Suppose you want to use functions in a class, just write the following code in its constructor function
  ```C#
    public class Test
    	{
    		private readonly PublicFunction.Converter.IDatatableConvertor iDatatableConvertor;
    		public BeforeLoginService(PublicFunction.Converter.IDatatableConvertor DatatableConvertor)
    		{
    		    iDatatableConvertor = DatatableConvertor;
    		}
    	}
  ```

  
# DataTable Utilities Documentation

This documentation provides detailed descriptions and examples for various data conversion functions in C#. These functions help you convert data between different formats such as JSON, XML, and CSV.

## Functions

### 1. DataTableToJson
```csharp
string DataTableToJson(DataTable _DataTable);

```

This function converts a DataTable to a JSON string.

**Parameters:**

-   `_DataTable`: The DataTable object to be converted to JSON.

**Returns:**

-   A JSON string representing the data in the DataTable.

**Example:**

```csharp
DataTable dt = new DataTable();
// Fill the DataTable with data
string json = DataTableToJson(dt);
Console.WriteLine(json);

```

### 2. JsonToDataTable

```csharp
DataTable JsonToDataTable(string Json);

```

This function converts a JSON string to a DataTable.

**Parameters:**

-   `Json`: The JSON string to be converted to a DataTable.

**Returns:**

-   A DataTable object representing the data in the JSON string.

**Example:**

```csharp
string json = "{...}"; // JSON string
DataTable dt = JsonToDataTable(json);

```

### 3. DataTableToDictionary

```csharp
List<Dictionary<string, object>> DataTableToDictionary(DataTable _DataTable);

```

This function converts a DataTable to a list of dictionaries.

**Parameters:**

-   `_DataTable`: The DataTable object to be converted to a list of dictionaries.

**Returns:**

-   A list of dictionaries, where each dictionary represents a row in the DataTable.

**Example:**

```csharp
DataTable dt = new DataTable();
// Fill the DataTable with data
List<Dictionary<string, object>> list = DataTableToDictionary(dt);

```

### 4. DataTableToDictionary (by reference)

```csharp
List<Dictionary<string, object>> DataTableToDictionary(ref DataTable _DataTable);

```

This function is similar to the previous one but takes the DataTable by reference.

**Parameters:**

-   `_DataTable`: The DataTable object to be converted to a list of dictionaries (by reference).

**Returns:**

-   A list of dictionaries, where each dictionary represents a row in the DataTable.

**Example:**

```csharp
DataTable dt = new DataTable();
// Fill the DataTable with data
List<Dictionary<string, object>> list = DataTableToDictionary(ref dt);

```

### 5. DataTableToXml

```csharp
string DataTableToXml(DataTable _DataTable);

```

This function converts a DataTable to an XML string.

**Parameters:**

-   `_DataTable`: The DataTable object to be converted to XML.

**Returns:**

-   An XML string representing the data in the DataTable.

**Example:**

```csharp
DataTable dt = new DataTable();
// Fill the DataTable with data
string xml = DataTableToXml(dt);
Console.WriteLine(xml);

```

### 6. XmlToDataTable

```csharp
DataTable XmlToDataTable(string Xml);

```

This function converts an XML string to a DataTable.

**Parameters:**

-   `Xml`: The XML string to be converted to a DataTable.

**Returns:**

-   A DataTable object representing the data in the XML string.

**Example:**

```csharp
string xml = "<DataTable>...</DataTable>"; // XML string
DataTable dt = XmlToDataTable(xml);

```

### 7. DataTableToCsv

```csharp
string DataTableToCsv(DataTable _DataTable);

```

This function converts a DataTable to a CSV string.

**Parameters:**

-   `_DataTable`: The DataTable object to be converted to CSV.

**Returns:**

-   A CSV string representing the data in the DataTable.

**Example:**

```csharp
DataTable dt = new DataTable();
// Fill the DataTable with data
string csv = DataTableToCsv(dt);
Console.WriteLine(csv);

```



# PublicFunction.Converter.DateTime.DateConverter

To implement a date conversion interface in C#, we can define an interface that converts dates between different formats (e.g., DateTime, Unix time, ISO-8601, etc.). Below is an example of how you could structure this, including comments and explanations.

### Explanation:

1.  **`IDateConverter` Interface**: Defines several methods for converting between different date formats.
    
    -   `ToUnixTimestamp(DateTime dateTime)`: Converts `DateTime` to Unix timestamp.
    -   `FromUnixTimestamp(long unixTimestamp)`: Converts Unix timestamp back to `DateTime`.
    -   `ToIso8601(DateTime dateTime)`: Converts `DateTime` to ISO-8601 string.
    -   `FromIso8601(string iso8601String)`: Converts ISO-8601 string to `DateTime`.
    -   `ToCustomFormat(DateTime dateTime, string format)`: Converts `DateTime` to a custom format.
    -   `FromCustomFormat(string dateString, string format)`: Converts custom format string back to `DateTime`.
2.  **`DateConverter` Class**: Implements the methods defined in `IDateConverter`.
    

This solution provides flexibility to convert dates between various formats using the methods from the interface.


## how to use the IDateConverter interface

Here's a guide on how to use the IDateConverter interface and its implementation DateConverter in your C# projects. This explanation includes examples demonstrating how to perform various date conversions using the provided interface.



