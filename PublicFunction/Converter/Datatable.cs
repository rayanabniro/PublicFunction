using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PublicFunction.Converter
{
    interface IDatatableConvertor
    {
        public string DataTableToJson(DataTable _DataTable);
        public DataTable JsonToDataTable(string Json);
        public List<Dictionary<string, object>> DataTableToDictionary(DataTable _DataTable);
        public List<Dictionary<string, object>> DataTableToDictionary(ref DataTable _DataTable);
    }
    public class DatatableConvertor: IDatatableConvertor
    {
        public string DataTableToJson(DataTable _DataTable)
        {
            try
            {
                if (_DataTable == null)
                {
                    return null;
                }

                return new Json().Serialize(DataTableToDictionary(ref _DataTable));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable JsonToDataTable(string Json)
        {
            if (Json == "")
            {
                return null;
            }

            List<Dictionary<string, object>> list = new Json().Deserialize<List<Dictionary<string, object>>>(Json);
            if (list.Count == 0)
            {
                return null;
            }

            DataTable result = new DataTable();
            result.Columns.AddRange((from r in list.First()
                                     select new DataColumn(r.Key, r.Value.GetType())).ToArray());
            list.ForEach(delegate (Dictionary<string, object> r)
            {
                result.Rows.Add(r.Select<KeyValuePair<string, object>, object>((KeyValuePair<string, object> c) => c.Value).Cast<object>().ToArray());
            });
            return result;
        }

        public List<Dictionary<string, object>> DataTableToDictionary(DataTable _DataTable)
        {
            try
            {
                if (_DataTable == null)
                {
                    return null;
                }

                return DataTableToDictionary(ref _DataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dictionary<string, object>> DataTableToDictionary(ref DataTable _DataTable)
        {
            try
            {
                if (_DataTable == null)
                {
                    return null;
                }

                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                foreach (DataRow row in _DataTable.Rows)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    foreach (DataColumn column in _DataTable.Columns)
                    {
                        Type type = row[column].GetType();
                        if (type == typeof(string))
                        {
                            string text = row[column].ToString();
                            if (Regex.Matches(text, "(\"\\w+\":(\\d+|\"\\w+\"|\"(?<=\\\")([^\\s].*?)(?=\\\")\"|true|false|null))").Count > 0 && text.IndexOf("[") != -1 && text.IndexOf("]") != -1)
                            {
                                List<Dictionary<string, object>> value = new Json().Deserialize<List<Dictionary<string, object>>>(text);
                                dictionary.Add(column.ColumnName, value);
                            }
                            else
                            {
                                dictionary.Add(column.ColumnName, row[column]);
                            }
                        }
                        else if (type == typeof(DBNull))
                        {
                            dictionary.Add(column.ColumnName, null);
                        }
                        else
                        {
                            dictionary.Add(column.ColumnName, row[column]);
                        }
                    }

                    list.Add(dictionary);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
