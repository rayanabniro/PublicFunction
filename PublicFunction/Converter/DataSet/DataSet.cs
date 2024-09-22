using System;
using System.Collections.Generic;
using System.Data;

namespace PublicFunction.Converter
{
    interface IDataSetConverter
    {
        public string DataSetToJson(DataSet _DataSet);
        public List<List<Dictionary<string, object>>> DataSetToList(DataSet _DataSet);
        public List<List<Dictionary<string, object>>> DataSetToList(ref DataSet _DataSet);
    }
    public class DataSetConverter : IDataSetConverter
    {
        public string DataSetToJson(DataSet _DataSet)
        {
            try
            {
                if (_DataSet == null)
                {
                    return null;
                }

                return new Json().Serialize(_DataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<List<Dictionary<string, object>>> DataSetToList(DataSet _DataSet)
        {
            try
            {
                if (_DataSet == null)
                {
                    return null;
                }

                return DataSetToList(ref _DataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<List<Dictionary<string, object>>> DataSetToList(ref DataSet _DataSet)
        {
            try
            {
                if (_DataSet == null)
                {
                    return null;
                }

                List<List<Dictionary<string, object>>> list = new List<List<Dictionary<string, object>>>();
                for (int i = 0; i < _DataSet.Tables.Count; i++)
                {
                    list.Add(new DatatableConvertor().DataTableToDictionary(_DataSet.Tables[i]));
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
