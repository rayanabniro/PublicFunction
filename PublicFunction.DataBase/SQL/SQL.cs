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
            get { return _SqlCommand.CommandText; }
            set { _SqlCommand = new(value, _SqlConnectionMirror) { CommandType = CommandType.StoredProcedure }; }
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
            foreach (var prop in typeof(T).GetProperties())
            {
                var value = prop.GetValue(TModel);
                _SqlCommand.Parameters.Add(new SqlParameter("@" + prop.Name, value ?? DBNull.Value));
            }
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
            catch (Exception ex)
            {
                throw ex;
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
                DataTable dt = new DataTable();
                _SqlConnectionMirror.Open();
                SqlDataAdapter da = new SqlDataAdapter(_SqlCommand);
                da.Fill(dt);
                _SqlConnectionMirror.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Dictionary<string, object>> SelectList()
        {
            try
            {
                DataTable dt = Select();
                return dt.AsEnumerable().Select(row => dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col])).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T SelectModel<T>() where T : SQLDataModel
        {
            try
            {
                DataTable dt = Select();
                if (dt.Rows.Count == 0) return default;
                T model = Activator.CreateInstance<T>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (dt.Columns.Contains(prop.Name))
                    {
                        prop.SetValue(model, dt.Rows[0][prop.Name] == DBNull.Value ? null : dt.Rows[0][prop.Name]);
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet MultiSelect()
        {
            try
            {
                DataSet ds = new DataSet();
                _SqlConnectionMirror.Open();
                SqlDataAdapter da = new SqlDataAdapter(_SqlCommand);
                da.Fill(ds);
                _SqlConnectionMirror.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<List<Dictionary<string, object>>> MultiSelectList()
        {
            try
            {
                DataSet ds = MultiSelect();
                return ds.Tables.Cast<DataTable>().Select(table => table.AsEnumerable().Select(row => table.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col])).ToList()).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T MultiSelectModel<T>(string[] tableNames) where T : SQLDataModel
        {
            try
            {
                DataSet ds = MultiSelect();
                var model = Activator.CreateInstance<T>();
                foreach (var tableName in tableNames)
                {
                    if (ds.Tables.Contains(tableName))
                    {
                        var table = ds.Tables[tableName];
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            if (table.Columns.Contains(prop.Name))
                            {
                                prop.SetValue(model, table.Rows[0][prop.Name] == DBNull.Value ? null : table.Rows[0][prop.Name]);
                            }
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Insert<T>(string tableName, T model) where T : SQLDataModel
        {
            try
            {
                var columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
                var values = string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name));
                var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
                _SqlCommand = new SqlCommand(query, _SqlConnection);
                foreach (var prop in typeof(T).GetProperties())
                {
                    _SqlCommand.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(model) ?? DBNull.Value);
                }
                _SqlConnection.Open();
                _SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _SqlConnection.Close();
            }
        }
        public bool Update<T>(string tableName, string id, T model) where T : SQLDataModel
        {
            try
            {
                var setClause = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name + " = @" + p.Name));
                var query = $"UPDATE {tableName} SET {setClause} WHERE ID = @ID";
                _SqlCommand = new SqlCommand(query, _SqlConnection);
                _SqlCommand.Parameters.AddWithValue("@ID", id);
                foreach (var prop in typeof(T).GetProperties())
                {
                    _SqlCommand.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(model) ?? DBNull.Value);
                }
                _SqlConnection.Open();
                _SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _SqlConnection.Close();
            }
        }
        public bool Delete(string tableName, string id)
        {
            try
            {
                var query = $"DELETE FROM {tableName} WHERE ID = @ID";
                _SqlCommand = new SqlCommand(query, _SqlConnection);
                _SqlCommand.Parameters.AddWithValue("@ID", id);
                _SqlConnection.Open();
                _SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _SqlConnection.Close();
            }
        }
    }
}

