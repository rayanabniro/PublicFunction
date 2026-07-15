using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PublicFunction.AsyncDataBase
{
    public class SQLAsync
    {
        public class SQLStoredProcedureModel { }
        public class SQLDataModel { }

        public interface ISQLServiceAsync
        {
            string StoredProcedureName { get; set; }
            void AddParameter(string Name, SqlDbType DbType, object Data);
            void AddParameter(string Name, object Data);
            void AddParameterS<T>(T TModel) where T : SQLStoredProcedureModel;
            Task<bool> AddAsync();
            Task<DataTable> SelectAsync();
            Task<List<Dictionary<string, object>>> SelectListAsync();
            Task<T> SelectModelAsync<T>() where T : SQLDataModel;
            Task<DataSet> MultiSelectAsync();
            Task<List<List<Dictionary<string, object>>>> MultiSelectListAsync();
            Task<T> MultiSelectModelAsync<T>(string[] tableNames) where T : SQLDataModel;
            Task<bool> InsertAsync<T>(string tableName, T Model) where T : SQLDataModel;
            Task<bool> UpdateAsync<T>(string tableName, string ID, T Model) where T : SQLDataModel;
            Task<bool> DeleteAsync(string tableName, string ID);
        }

        public class SQLServiceAsync : ISQLServiceAsync
        {
            private readonly IConfiguration Configuration;
            private readonly string _connectionString;
            private readonly string _connectionStringMirror;
            private SqlCommand _sqlCommand;
            private SqlParameterCollection _parameters;

            public SQLServiceAsync(IConfiguration configuration)
            {
                Configuration = configuration;
                _connectionString = Configuration["PublicFunction:DataBase:SQL:SQLServiceAsync:ConnectionString"]?.ToString();
                _connectionStringMirror = Configuration["PublicFunction:DataBase:SQL:SQLServiceAsync:ConnectionStringMirror"]?.ToString();
                _sqlCommand = new SqlCommand();
                _parameters = _sqlCommand.Parameters;
            }

            public string StoredProcedureName
            {
                get => _sqlCommand.CommandText;
                set
                {
                    _sqlCommand.CommandText = value;
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                }
            }

            public void AddParameter(string Name, SqlDbType DbType, object Data)
            {
                _parameters.Add(new SqlParameter(Name, DbType) { Value = Data ?? DBNull.Value });
            }

            public void AddParameter(string Name, object Data)
            {
                _parameters.Add(new SqlParameter(Name, Data ?? DBNull.Value));
            }

            public void AddParameterS<T>(T TModel) where T : SQLStoredProcedureModel
            {
                foreach (var prop in typeof(T).GetProperties())
                {
                    var value = prop.GetValue(TModel);
                    _parameters.Add(new SqlParameter("@" + prop.Name, value ?? DBNull.Value));
                }
            }

            private SqlConnection CreateConnection(string connectionString)
            {
                return new SqlConnection(connectionString);
            }

            private void PrepareCommand(SqlConnection connection)
            {
                _sqlCommand.Connection = connection;
                _sqlCommand.CommandType = string.IsNullOrEmpty(_sqlCommand.CommandText) ? CommandType.Text : _sqlCommand.CommandType;
            }

            public async Task<bool> AddAsync()
            {
                try
                {
                    using var connection = CreateConnection(_connectionString);
                    PrepareCommand(connection);
                    await connection.OpenAsync();
                    await _sqlCommand.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error executing AddAsync", ex);
                }
                finally
                {
                    _parameters.Clear();
                }
            }

            public async Task<DataTable> SelectAsync()
            {
                var dt = new DataTable();
                try
                {
                    using var connection = CreateConnection(_connectionStringMirror);
                    PrepareCommand(connection);
                    await connection.OpenAsync();
                    using var da = new SqlDataAdapter(_sqlCommand);
                    da.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error executing SelectAsync", ex);
                }
                finally
                {
                    _parameters.Clear();
                }
            }

            public async Task<List<Dictionary<string, object>>> SelectListAsync()
            {
                try
                {
                    var dt = await SelectAsync();
                    return dt.AsEnumerable()
                        .Select(row => dt.Columns.Cast<DataColumn>()
                            .ToDictionary(col => col.ColumnName, col => row[col]))
                        .ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error executing SelectListAsync", ex);
                }
            }

            public async Task<T> SelectModelAsync<T>() where T : SQLDataModel
            {
                try
                {
                    var dt = await SelectAsync();
                    if (dt.Rows.Count == 0) return default;

                    var model = Activator.CreateInstance<T>();
                    var row = dt.Rows[0];

                    foreach (var prop in typeof(T).GetProperties())
                    {
                        if (dt.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                        {
                            prop.SetValue(model, row[prop.Name]);
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error executing SelectModelAsync", ex);
                }
            }

            public async Task<DataSet> MultiSelectAsync()
            {
                var ds = new DataSet();
                try
                {
                    using var connection = CreateConnection(_connectionStringMirror);
                    PrepareCommand(connection);
                    await connection.OpenAsync();
                    using var da = new SqlDataAdapter(_sqlCommand);
                    da.Fill(ds);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error executing MultiSelectAsync", ex);
                }
                finally
                {
                    _parameters.Clear();
                }
            }

            public async Task<List<List<Dictionary<string, object>>>> MultiSelectListAsync()
            {
                try
                {
                    var ds = await MultiSelectAsync();
                    return ds.Tables.Cast<DataTable>()
                        .Select(table => table.AsEnumerable()
                            .Select(row => table.Columns.Cast<DataColumn>()
                                .ToDictionary(col => col.ColumnName, col => row[col]))
                            .ToList())
                        .ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error executing MultiSelectListAsync", ex);
                }
            }

            public async Task<T> MultiSelectModelAsync<T>(string[] tableNames) where T : SQLDataModel
            {
                try
                {
                    var ds = await MultiSelectAsync();
                    var model = Activator.CreateInstance<T>();

                    foreach (var tableName in tableNames)
                    {
                        if (!ds.Tables.Contains(tableName)) continue;

                        var table = ds.Tables[tableName];
                        if (table.Rows.Count == 0) continue;

                        var row = table.Rows[0];
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            if (table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                            {
                                prop.SetValue(model, row[prop.Name]);
                            }
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error executing MultiSelectModelAsync", ex);
                }
            }

            public async Task<bool> InsertAsync<T>(string tableName, T model) where T : SQLDataModel
            {
                try
                {
                    var properties = typeof(T).GetProperties();
                    var columns = string.Join(", ", properties.Select(p => p.Name));
                    var values = string.Join(", ", properties.Select(p => "@" + p.Name));
                    var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                    using var connection = CreateConnection(_connectionString);
                    using var cmd = new SqlCommand(query, connection);

                    foreach (var prop in properties)
                    {
                        cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(model) ?? DBNull.Value);
                    }

                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error inserting into {tableName}", ex);
                }
            }

            public async Task<bool> UpdateAsync<T>(string tableName, string id, T model) where T : SQLDataModel
            {
                try
                {
                    var properties = typeof(T).GetProperties();
                    var setClause = string.Join(", ", properties.Select(p => p.Name + " = @" + p.Name));
                    var query = $"UPDATE {tableName} SET {setClause} WHERE ID = @ID";

                    using var connection = CreateConnection(_connectionString);
                    using var cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ID", id);

                    foreach (var prop in properties)
                    {
                        cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(model) ?? DBNull.Value);
                    }

                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating {tableName}", ex);
                }
            }

            public async Task<bool> DeleteAsync(string tableName, string id)
            {
                try
                {
                    var query = $"DELETE FROM {tableName} WHERE ID = @ID";
                    using var connection = CreateConnection(_connectionString);
                    using var cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ID", id);

                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting from {tableName}", ex);
                }
            }
        }
    }
}