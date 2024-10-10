using System.Collections.Generic;
using System.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace PublicFunction.Converter
{
    public interface IDataSetConverter
    {
        string DataSetToJson(DataSet dataSet);
        List<List<Dictionary<string, object>>> DataSetToList(DataSet dataSet);
        List<List<Dictionary<string, object>>> DataSetToList(ref DataSet dataSet);
        string DataSetToXml(DataSet dataSet);
        string DataSetToCsv(DataSet dataSet, string delimiter = ",");
        byte[] DataSetToExcel(DataSet dataSet);
        string DataSetToHtml(DataSet dataSet);
    }
    public class DataSetConverter : IDataSetConverter
    {
        /// <summary>
        /// Converts a DataSet to a JSON string.
        /// </summary>
        public string DataSetToJson(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet), "DataSet cannot be null.");

            return JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Converts a DataSet to a list of lists of dictionaries.
        /// </summary>
        public List<List<Dictionary<string, object>>> DataSetToList(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet), "DataSet cannot be null.");

            var result = new List<List<Dictionary<string, object>>>();

            foreach (DataTable table in dataSet.Tables)
            {
                var tableList = new List<Dictionary<string, object>>();

                foreach (DataRow row in table.Rows)
                {
                    var rowDict = new Dictionary<string, object>();
                    foreach (DataColumn column in table.Columns)
                    {
                        rowDict[column.ColumnName] = row[column];
                    }
                    tableList.Add(rowDict);
                }

                result.Add(tableList);
            }

            return result;
        }

        /// <summary>
        /// Converts a DataSet to a list of lists of dictionaries using a reference parameter.
        /// </summary>
        public List<List<Dictionary<string, object>>> DataSetToList(ref DataSet dataSet)
        {
            // In this implementation, the reference parameter is not modified.
            return DataSetToList(dataSet);
        }

        /// <summary>
        /// Converts a DataSet to an XML string.
        /// </summary>
        public string DataSetToXml(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet), "DataSet cannot be null.");

            using (StringWriter sw = new StringWriter())
            {
                // Removed the third parameter 'false' as it may not be supported
                dataSet.WriteXml(sw, XmlWriteMode.IgnoreSchema);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Converts a DataSet to a CSV string.
        /// </summary>
        public string DataSetToCsv(DataSet dataSet, string delimiter = ",")
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet), "DataSet cannot be null.");

            StringBuilder sb = new StringBuilder();

            foreach (DataTable table in dataSet.Tables)
            {
                // Add table name as a header
                sb.AppendLine($"Table: {table.TableName}");

                // Add column headers
                IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>()
                    .Select(column => EscapeCsvValue(column.ColumnName, delimiter));
                sb.AppendLine(string.Join(delimiter, columnNames));

                // Add rows
                foreach (DataRow row in table.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray
                        .Select(field => EscapeCsvValue(field?.ToString() ?? string.Empty, delimiter));
                    sb.AppendLine(string.Join(delimiter, fields));
                }

                sb.AppendLine(); // Empty line to separate tables
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a DataSet to an Excel file in byte array format using XML Spreadsheet 2003.
        /// </summary>
        public byte[] DataSetToExcel(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet), "DataSet cannot be null.");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sb.AppendLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            sb.AppendLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            sb.AppendLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");

            foreach (DataTable table in dataSet.Tables)
            {
                sb.AppendLine($"  <Worksheet ss:Name=\"{EscapeXml(table.TableName)}\">");
                sb.AppendLine("    <Table>");

                // Add column headers
                sb.AppendLine("      <Row>");
                foreach (DataColumn column in table.Columns)
                {
                    sb.AppendLine($"        <Cell><Data ss:Type=\"String\">{EscapeXml(column.ColumnName)}</Data></Cell>");
                }
                sb.AppendLine("      </Row>");

                // Add rows
                foreach (DataRow row in table.Rows)
                {
                    sb.AppendLine("      <Row>");
                    foreach (var item in row.ItemArray)
                    {
                        string dataType = "String";
                        string dataValue = EscapeXml(item?.ToString() ?? string.Empty);

                        if (item is int || item is long || item is short || item is byte)
                        {
                            dataType = "Number";
                        }
                        else if (item is float || item is double || item is decimal)
                        {
                            dataType = "Number";
                        }
                        else if (item is DateTime)
                        {
                            dataType = "DateTime";
                            DateTime dt = (DateTime)item;
                            dataValue = dt.ToString("s") + "Z"; // ISO 8601 format
                        }

                        sb.AppendLine($"        <Cell><Data ss:Type=\"{dataType}\">{dataValue}</Data></Cell>");
                    }
                    sb.AppendLine("      </Row>");
                }

                sb.AppendLine("    </Table>");
                sb.AppendLine("  </Worksheet>");
            }

            sb.AppendLine("</Workbook>");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        /// <summary>
        /// Converts a DataSet to an HTML string.
        /// </summary>
        public string DataSetToHtml(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet), "DataSet cannot be null.");

            StringBuilder sb = new StringBuilder();

            foreach (DataTable table in dataSet.Tables)
            {
                sb.AppendLine($"<h2>{System.Net.WebUtility.HtmlEncode(table.TableName)}</h2>");
                sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");

                // Add table headers
                sb.AppendLine("<thead><tr>");
                foreach (DataColumn column in table.Columns)
                {
                    sb.AppendLine($"<th>{System.Net.WebUtility.HtmlEncode(column.ColumnName)}</th>");
                }
                sb.AppendLine("</tr></thead>");

                // Add table rows
                sb.AppendLine("<tbody>");
                foreach (DataRow row in table.Rows)
                {
                    sb.AppendLine("<tr>");
                    foreach (var item in row.ItemArray)
                    {
                        sb.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(item?.ToString() ?? string.Empty)}</td>");
                    }
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</tbody>");

                sb.AppendLine("</table><br/>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Escapes a value for CSV format.
        /// </summary>
        private string EscapeCsvValue(string value, string delimiter)
        {
            if (value.Contains(delimiter) || value.Contains("\"") || value.Contains("\n"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }

        /// <summary>
        /// Escapes special characters for XML.
        /// </summary>
        private string EscapeXml(string value)
        {
            return System.Security.SecurityElement.Escape(value);
        }
    }
}
