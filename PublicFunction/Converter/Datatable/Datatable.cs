using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public string DataTableToJson(DataTable _DataTable)
        {
            return JsonConvert.SerializeObject(_DataTable);
        }

        public DataTable JsonToDataTable(string Json)
        {
            return JsonConvert.DeserializeObject<DataTable>(Json);
        }

        public List<Dictionary<string, object>> DataTableToDictionary(DataTable _DataTable)
        {
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

        public List<Dictionary<string, object>> DataTableToDictionary(ref DataTable _DataTable)
        {
            return DataTableToDictionary(_DataTable);
        }

        public string DataTableToXml(DataTable _DataTable)
        {
            using (var writer = new StringWriter())
            {
                _DataTable.WriteXml(writer, XmlWriteMode.WriteSchema, false);
                return writer.ToString();
            }
        }

        public DataTable XmlToDataTable(string Xml)
        {
            var dataTable = new DataTable();
            using (var reader = new StringReader(Xml))
            {
                dataTable.ReadXml(reader);
            }
            return dataTable;
        }

        public string DataTableToCsv(DataTable _DataTable)
        {
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
    }
}
