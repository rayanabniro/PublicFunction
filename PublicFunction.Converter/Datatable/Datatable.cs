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
    public class DatatableConverter
    {
        /// <summary>
        /// Provides comprehensive DataTable conversion and serialization services.
        /// This interface defines methods for converting DataTable objects to various formats
        /// including JSON, XML, CSV, and Dictionary structures.
        /// </summary>
        public interface IDatatable
        {
            /// <summary>
            /// Converts a DataTable to a JSON string representation.
            /// Uses System.Text.Json for serialization with indented formatting.
            /// </summary>
            /// <param name="_DataTable">The DataTable object to convert to JSON</param>
            /// <returns>A JSON string representing the DataTable data</returns>
            /// <exception cref="ArgumentNullException">Thrown when the DataTable is null</exception>
            /// <example>
            /// <code>
            /// var json = datatableService.DataTableToJson(myDataTable);
            /// // Output: [{"Id":1,"Name":"Product A","Price":100.5},...]
            /// </code>
            /// </example>
            public string DataTableToJson(DataTable _DataTable);

            /// <summary>
            /// Converts a JSON string back to a DataTable object.
            /// Deserializes the JSON array of objects into a DataTable with automatic column detection.
            /// </summary>
            /// <param name="Json">The JSON string to convert to DataTable</param>
            /// <returns>A DataTable populated with data from the JSON string</returns>
            /// <exception cref="ArgumentNullException">Thrown when the JSON string is null or empty</exception>
            /// <example>
            /// <code>
            /// string json = "[{\"Id\":1,\"Name\":\"Product A\"}]";
            /// DataTable table = datatableService.JsonToDataTable(json);
            /// </code>
            /// </example>
            public DataTable JsonToDataTable(string Json);

            /// <summary>
            /// Converts a DataTable to a list of dictionaries.
            /// Each dictionary represents a row with column names as keys and row values as values.
            /// </summary>
            /// <param name="_DataTable">The DataTable to convert</param>
            /// <returns>List of dictionaries where each dictionary represents a DataRow</returns>
            /// <exception cref="ArgumentNullException">Thrown when the DataTable is null</exception>
            /// <example>
            /// <code>
            /// var dictList = datatableService.DataTableToDictionary(table);
            /// // Result: List&lt;Dictionary&lt;string, object&gt;&gt; where each dictionary has column:value pairs
            /// </code>
            /// </example>
            public List<Dictionary<string, object>> DataTableToDictionary(DataTable _DataTable);

            /// <summary>
            /// Converts a DataTable to a list of dictionaries using a reference parameter.
            /// This overload provides the same functionality as the non-ref version but allows
            /// passing the DataTable by reference for scenarios where the reference might be modified.
            /// </summary>
            /// <param name="_DataTable">The DataTable reference to convert</param>
            /// <returns>List of dictionaries representing the DataTable data</returns>
            /// <exception cref="ArgumentNullException">Thrown when the DataTable is null</exception>
            public List<Dictionary<string, object>> DataTableToDictionary(ref DataTable _DataTable);

            /// <summary>
            /// Converts a DataTable to an XML string including schema information.
            /// Uses the standard DataTable.WriteXml method with WriteSchema option.
            /// </summary>
            /// <param name="_DataTable">The DataTable to convert to XML</param>
            /// <returns>XML string representation of the DataTable with schema</returns>
            /// <exception cref="ArgumentNullException">Thrown when the DataTable is null</exception>
            /// <example>
            /// <code>
            /// string xml = datatableService.DataTableToXml(myDataTable);
            /// // Returns well-formed XML with both data and schema
            /// </code>
            /// </example>
            public string DataTableToXml(DataTable _DataTable);

            /// <summary>
            /// Converts an XML string back to a DataTable object.
            /// Reads the XML and reconstructs the DataTable with its data and schema.
            /// </summary>
            /// <param name="Xml">The XML string to convert to DataTable</param>
            /// <returns>A DataTable reconstructed from the XML string</returns>
            /// <exception cref="ArgumentNullException">Thrown when the XML string is null or empty</exception>
            /// <exception cref="System.Xml.XmlException">Thrown when the XML is malformed</exception>
            /// <example>
            /// <code>
            /// string xml = "&lt;DocumentElement&gt;&lt;Table&gt;&lt;Id&gt;1&lt;/Id&gt;&lt;/Table&gt;&lt;/DocumentElement&gt;";
            /// DataTable table = datatableService.XmlToDataTable(xml);
            /// </code>
            /// </example>
            public DataTable XmlToDataTable(string Xml);

            /// <summary>
            /// Converts a DataTable to a CSV (Comma-Separated Values) string.
            /// The first row contains column headers, followed by data rows.
            /// </summary>
            /// <param name="_DataTable">The DataTable to convert to CSV</param>
            /// <returns>A CSV string with column headers and data rows</returns>
            /// <exception cref="ArgumentNullException">Thrown when the DataTable is null</exception>
            /// <example>
            /// <code>
            /// string csv = datatableService.DataTableToCsv(myDataTable);
            /// // Output:
            /// // Id,Name,Price
            /// // 1,Product A,100.5
            /// // 2,Product B,200.75
            /// </code>
            /// </example>
            public string DataTableToCsv(DataTable _DataTable);
        }
        public class DatatableService : IDatatable
        {
            /// <summary>
            /// Converts a DataTable to a JSON string using System.Text.Json.
            /// </summary>
            public string DataTableToJson(DataTable _DataTable)
            {
                if (_DataTable == null)
                    throw new ArgumentNullException(nameof(_DataTable));

                var list = DataTableToDictionary(_DataTable);
                return JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            }

            /// <summary>
            /// Converts a JSON string to a DataTable using System.Text.Json.
            /// </summary>
            public DataTable JsonToDataTable(string Json)
            {
                if (string.IsNullOrEmpty(Json))
                    throw new ArgumentNullException(nameof(Json));

                var list = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(Json);
                if (list == null || list.Count == 0)
                    return new DataTable();

                return DictionaryListToDataTable(list);
            }

            /// <summary>
            /// Converts a DataTable to a list of dictionaries.
            /// </summary>
            public List<Dictionary<string, object>> DataTableToDictionary(DataTable _DataTable)
            {
                if (_DataTable == null)
                    throw new ArgumentNullException(nameof(_DataTable));

                var list = new List<Dictionary<string, object>>();

                foreach (DataRow row in _DataTable.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn col in _DataTable.Columns)
                    {
                        dict[col.ColumnName] = row[col];
                    }
                    list.Add(dict);
                }

                return list;
            }

            /// <summary>
            /// Converts a DataTable to a list of dictionaries using a reference parameter.
            /// </summary>
            public List<Dictionary<string, object>> DataTableToDictionary(ref DataTable _DataTable)
            {
                return DataTableToDictionary(_DataTable);
            }

            /// <summary>
            /// Converts a DataTable to an XML string.
            /// </summary>
            public string DataTableToXml(DataTable _DataTable)
            {
                if (_DataTable == null)
                    throw new ArgumentNullException(nameof(_DataTable));

                using (var writer = new StringWriter())
                {
                    _DataTable.WriteXml(writer, XmlWriteMode.WriteSchema);
                    return writer.ToString();
                }
            }

            /// <summary>
            /// Converts an XML string to a DataTable.
            /// </summary>
            public DataTable XmlToDataTable(string Xml)
            {
                if (string.IsNullOrEmpty(Xml))
                    throw new ArgumentNullException(nameof(Xml));

                var dataTable = new DataTable();
                using (var reader = new StringReader(Xml))
                {
                    dataTable.ReadXml(reader);
                }

                return dataTable;
            }

            /// <summary>
            /// Converts a DataTable to a CSV string.
            /// </summary>
            public string DataTableToCsv(DataTable _DataTable)
            {
                if (_DataTable == null)
                    throw new ArgumentNullException(nameof(_DataTable));

                var csv = new StringBuilder();

                foreach (DataColumn column in _DataTable.Columns)
                {
                    csv.Append(column.ColumnName + ",");
                }

                csv.AppendLine();

                foreach (DataRow row in _DataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        csv.Append(item.ToString() + ",");
                    }

                    csv.AppendLine();
                }

                return csv.ToString();
            }

            /// <summary>
            /// Helper method to convert a list of dictionaries to a DataTable.
            /// </summary>
            private DataTable DictionaryListToDataTable(List<Dictionary<string, object>> list)
            {
                var dataTable = new DataTable();

                if (list == null || list.Count == 0)
                    return dataTable;

                // Add columns
                foreach (var key in list[0].Keys)
                {
                    dataTable.Columns.Add(key);
                }

                // Add rows
                foreach (var dict in list)
                {
                    var row = dataTable.NewRow();
                    foreach (var key in dict.Keys)
                    {
                        row[key] = dict[key] ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
        }
    }
    
}
