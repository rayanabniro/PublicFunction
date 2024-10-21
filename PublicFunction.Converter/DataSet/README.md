# PublicFunction.Converter

In this **class**, we have infrastructures that can **centrally control** For Converter **dataSet**

## Use in .netCore
- in **Program.cs**  Add This dependence
  ```C#
    builder.Services.AddScoped<PublicFunction.Converter.IDataSetConverter, PublicFunction.Converter.DataSetConverter>();
  ```
- Suppose you want to use functions in a class, just write the following code in its constructor function
  ```C#
    public class Test
    	{
    		private readonly PublicFunction.Converter.IDataSetConverter DataSetConverter;
    		public BeforeLoginService(PublicFunction.Converter.IDataSetConverter dataSetConverter)
    		{
    		    DataSetConverter = dataSetConverter;
    		}
    	}
  ```

  
# DataSetConverter Class

The `DataSetConverter` class provides various methods to convert a `DataSet` into different formats such as JSON, list of dictionaries, XML, CSV, Excel, and HTML. Below is a detailed explanation of each method and how to use them.

## Methods

### DataSetToJson
Converts a `DataSet` to a JSON string.

```csharp
public string DataSetToJson(DataSet dataSet)

```

**Usage:**

```csharp
DataSetConverter converter = new DataSetConverter();
string json = converter.DataSetToJson(yourDataSet);

```

### DataSetToList

Converts a `DataSet` to a list of lists of dictionaries.

```csharp
public List<List<Dictionary<string, object>>> DataSetToList(DataSet dataSet)

```

**Usage:**

```csharp
List<List<Dictionary<string, object>>> list = converter.DataSetToList(yourDataSet);

```

### DataSetToList (with ref parameter)

Converts a `DataSet` to a list of lists of dictionaries using a reference parameter.

```csharp
public List<List<Dictionary<string, object>>> DataSetToList(ref DataSet dataSet)

```

**Usage:**

```csharp
List<List<Dictionary<string, object>>> list = converter.DataSetToList(ref yourDataSet);

```

### DataSetToXml

Converts a `DataSet` to an XML string.

```csharp
public string DataSetToXml(DataSet dataSet)

```

**Usage:**

```csharp
string xml = converter.DataSetToXml(yourDataSet);

```

### DataSetToCsv

Converts a `DataSet` to a CSV string.

```csharp
public string DataSetToCsv(DataSet dataSet, string delimiter = ",")

```

**Usage:**

```csharp
string csv = converter.DataSetToCsv(yourDataSet);

```

### DataSetToExcel

Converts a `DataSet` to an Excel file in byte array format using XML Spreadsheet 2003.

```csharp
public byte[] DataSetToExcel(DataSet dataSet)

```

**Usage:**

```csharp
byte[] excelBytes = converter.DataSetToExcel(yourDataSet);

```

### DataSetToHtml

Converts a `DataSet` to an HTML string.

```csharp
public string DataSetToHtml(DataSet dataSet)

```

**Usage:**

```csharp
string html = converter.DataSetToHtml(yourDataSet);

```

## Helper Methods

### EscapeCsvValue

Escapes a value for CSV format.

```csharp
private string EscapeCsvValue(string value, string delimiter)

```

### EscapeXml

Escapes special characters for XML.

```csharp
private string EscapeXml(string value)

```

## Example Usage

```csharp
DataSetConverter converter = new DataSetConverter();
DataSet dataSet = GetYourDataSet(); // Assume this method retrieves your DataSet

// Convert to JSON
string json = converter.DataSetToJson(dataSet);

// Convert to List of Dictionaries
List<List<Dictionary<string, object>>> list = converter.DataSetToList(dataSet);

// Convert to XML
string xml = converter.DataSetToXml(dataSet);

// Convert to CSV
string csv = converter.DataSetToCsv(dataSet);

// Convert to Excel
byte[] excelBytes = converter.DataSetToExcel(dataSet);

// Convert to HTML
string html = converter.DataSetToHtml(dataSet);

```



