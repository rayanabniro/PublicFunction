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
    public interface IDatatableConvertor
    {
        string DataTableToJson(DataTable _DataTable);
        DataTable JsonToDataTable(string Json);
        List<Dictionary<string, object>> DataTableToDictionary(DataTable _DataTable);
        List<Dictionary<string, object>> DataTableToDictionary(ref DataTable _DataTable);
        string DataTableToXml(DataTable _DataTable);
        DataTable XmlToDataTable(string Xml);
        string DataTableToCsv(DataTable _DataTable);
    }

    public class DatatableConvertor : IDatatableConvertor
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
