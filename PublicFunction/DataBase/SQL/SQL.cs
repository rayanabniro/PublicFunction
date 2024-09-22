using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace PublicFunction.DataBase;
public class SQL
{
    public class SQLStoredProcedureModel { }
    public class SQLDataModel { }
    public interface ISQLService
    {
        public string StoredProcedureName { get; set; }
        public void AddParameter(string Name, SqlDbType DbType, object Data);
        public void AddParameter(string Name, object Data);
        public void AddParameterS<T>(T TModel) where T : SQLStoredProcedureModel;
        public bool Add();
        public DataTable Select();
        public List<Dictionary<string, object>> SelectList();
        public T SelectModel<T>() where T : SQLDataModel;
        public DataSet MultiSelect();
        public List<List<Dictionary<string, object>>> MultiSelectList();
        public T MultiSelectModel<T>(string[] tableName) where T : SQLDataModel;
        public bool Insert<T>(string tableName, T Model) where T : SQLDataModel;
        public bool Update<T>(string tableName, string ID, T Model) where T : SQLDataModel;
        public bool Delete(string tableName, string ID);
    }
    public class SQLService : ISQLService
    {
        private readonly IConfiguration Configuration;
        SqlConnection _SqlConnectionMirror, _SqlConnection;
        SqlCommand _SqlCommand;
        public SQLService(IConfiguration configuration)
        {
            Configuration = configuration;
            _SqlConnectionMirror = new SqlConnection(ConnectionStringMirror);
            _SqlConnection = new SqlConnection(ConnectionString);

        }
        private string ConnectionString
        {
            get
            {
                return Configuration["PublicFunction:DataBase:SQL:SQLService:ConnectionString"].ToString();
            }
        }
        private string ConnectionStringMirror
        {
            get
            {
                return Configuration["PublicFunction:DataBase:SQL:SQLService:ConnectionStringMirror"].ToString();
            }
        }
        public string StoredProcedureName
        {
            get { return _SqlCommand.CommandText; }   // get method
            set { _SqlCommand = new(value, _SqlConnectionMirror) { CommandType = CommandType.StoredProcedure }; }  // set method
        }
        public void AddParameter(string Name, SqlDbType DbType, object Data)
        {
            try
            {
                _SqlCommand.Parameters.Add(new SqlParameter(Name, DbType) { Value = Data });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddParameter(string Name, object Data)
        {
            try
            {
                _SqlCommand.Parameters.Add(new SqlParameter(Name, Data));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddParameterS<T>(T TModel) where T : SQLStoredProcedureModel
        {
        }
        public bool Add()
        {
            try
            {

                _SqlCommand.Connection = _SqlConnection;
                _SqlConnection.Open();
                _SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception EX)
            {

                throw EX;
            }
            finally
            {
                _SqlConnection.Close();
            }

        }
        public DataTable Select()
        {
            try
            {
                DataTable DT = new DataTable();
                _SqlConnectionMirror.Open();
                SqlDataAdapter da = new SqlDataAdapter(_SqlCommand);
                da.Fill(DT);
                _SqlConnectionMirror.Close();
                return DT;

            }
            catch (Exception EX)
            {

                throw EX;
            }
        }
        public List<Dictionary<string, object>> SelectList()
        {
            try
            {
                DataTable dt = Select();
                return new Converter.DatatableConvertor().DataTableToDictionary(dt);
            }
            catch (Exception EX)
            {

                throw EX;
            }
        }
        public T SelectModel<T>() where T : SQLDataModel
        {
            try
            {
                DataTable Data = Select();
                string Json = new Converter.DatatableConvertor().DataTableToJson(Data);
                return new Converter.Json().Deserialize<T>(Json);
            }
            catch (Exception EX)
            {

                throw EX;
            }
        }
        public DataSet MultiSelect()
        {
            try
            {
                DataSet DS = new DataSet();
                _SqlConnectionMirror.Open();
                SqlDataAdapter da = new SqlDataAdapter(_SqlCommand);
                da.Fill(DS);
                _SqlConnectionMirror.Close();
                return DS;

            }
            catch (Exception EX)
            {

                throw EX;
            }
        }

        public List<List<Dictionary<string, object>>> MultiSelectList()
        {
            try
            {
                DataSet DS = MultiSelect();
                return new Converter.DataSetConverter().DataSetToList(DS);

            }
            catch (Exception EX)
            {

                throw EX;
            }
        }
        public T MultiSelectModel<T>(string[] tableName) where T : SQLDataModel
        {
            try
            {
                DataSet Data = MultiSelect();
                string Json = new Converter.DataSetConverter().DataSetToJson(Data);
                for (int i = tableName.Length - 1; 0 <= i; i--)
                {
                    string TableName = i == 0 ? @"""Table""" : @"""Table" + i + @"""";
                    Json = Json.Replace(TableName, @"""" + tableName[i] + @"""");
                }
                return new Converter.Json().Deserialize<T>(Json);
            }
            catch (Exception EX)
            {

                throw EX;
            }
        }
        public bool Insert<T>(string tableName, T Model) where T : SQLDataModel
        {
            try
            {
                List<string> Columns = new List<string>();
                List<string> Value = new List<string>();
                Type _Type = Model.GetType();
                foreach (PropertyInfo pi in _Type.GetProperties())
                {
                    string Data = pi.GetValue(Model, null)?.ToString();
                    if (pi.PropertyType == typeof(bool))
                    {
                        if (Data.ToLower() == "true")
                        {
                            Data = "''1''";
                        }
                        else
                        {
                            Data = "''0''";
                        }
                    }
                    else if (pi.PropertyType == typeof(string))
                    {
                        Data = "N''" + Data + "''";
                    }
                    else
                    {
                        Data = "''" + Data + "''";
                    }

                    var DisplayName = pi.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name;
                    Columns.Add(pi.Name);
                    Value.Add(Data);
                }
                string ColumnsString = string.Join(",", Columns.Select(x => "[" + x + "]").ToArray());
                string ValueString = string.Join(",", Value.Select(x => x).ToArray());
                _SqlCommand = new SqlCommand()
                {
                    Connection = _SqlConnection,
                    CommandType = CommandType.Text,
                    CommandText = "EXEC sp_executesql N'INSERT INTO " + tableName + " (" + ColumnsString + ") VALUES (" + ValueString + ")'"
                };
                _SqlConnection.Open();
                _SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception EX)
            {

                throw EX;
            }
            finally
            {
                _SqlConnection.Close();
            }
        }
        public bool Update<T>(string tableName, string ID, T Model) where T : SQLDataModel
        {
            try
            {
                Dictionary<string, string> myDict = new Dictionary<string, string>();
                Type _Type = Model.GetType();
                foreach (PropertyInfo pi in _Type.GetProperties())
                {
                    object PV = pi.GetValue(Model, null);
                    if (PV != null)
                    {
                        string Value = PV?.ToString();
                        if (pi.PropertyType == typeof(bool))
                        {
                            if (Value.ToLower() == "true")
                            {
                                Value = "1";
                            }
                            else
                            {
                                Value = "0";
                            }
                        }
                        myDict.Add(pi.Name, Value);
                    }
                }
                string field = string.Join(",", myDict.Select(x => "[" + x.Key + "]=''" + x.Value + "''").ToArray());
                _SqlCommand = new SqlCommand()
                {
                    Connection = _SqlConnection,
                    CommandType = CommandType.Text,
                    CommandText = "EXEC sp_executesql N'UPDATE " + tableName + " SET " + field + " WHERE [ID] = N''" + ID + "'''"
                };
                _SqlConnection.Open();
                _SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception EX)
            {

                throw EX;
            }
            finally
            {
                _SqlConnection.Close();
            }
        }
        public bool Delete(string tableName, string ID)
        {
            try
            {
                _SqlCommand = new SqlCommand()
                {
                    Connection = _SqlConnection,
                    CommandType = CommandType.Text,
                    CommandText = "EXEC sp_executesql N'DELETE FROM " + tableName + "  WHERE [ID]=''" + ID + "'''"
                };
                _SqlConnection.Open();
                _SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception EX)
            {

                throw EX;
            }
            finally
            {
                _SqlConnection.Close();
            }
        }
    }
}
