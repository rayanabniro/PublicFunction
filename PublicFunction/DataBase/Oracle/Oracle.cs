using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using static PublicFunction.DataBase.Oracle;
using System.Data.Common;
using System.Data.Odbc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Microsoft.Extensions.Configuration;

namespace PublicFunction.DataBase
{
    public class Oracle
    {
        public abstract class OracleStoredProcedureModel
        {
            // Define common properties or methods for stored procedure models if needed
        }

        public abstract class OracleDataModel
        {
            // Define common properties or methods for data models if needed
        }
        public interface IOracleService
        {
            string StoredProcedureName { get; set; }
            void AddParameter(string name, OdbcType dbType, object data);
            void AddParameter(string name, object data);
            void AddParametersFromModel<T>(T model) where T : OracleStoredProcedureModel;
            bool Execute();
            DataTable Select();
            List<Dictionary<string, object>> SelectList();
            T SelectModel<T>() where T : OracleDataModel, new();
            DataSet MultiSelect();
            List<List<Dictionary<string, object>>> MultiSelectList();
            T MultiSelectModel<T>(string[] tableNames) where T : OracleDataModel, new();
            bool Insert<T>(string tableName, T model) where T : OracleDataModel;
            bool Update<T>(string tableName, string id, T model) where T : OracleDataModel;
            bool Delete(string tableName, string id);
        }
        public class OracleService : IOracleService
            {
                private readonly IConfiguration _configuration;
                private readonly OdbcConnection _oracleConnection;
                private OdbcCommand _oracleCommand;

                // Constructor to initialize the connection string from configuration
                public OracleService(IConfiguration configuration)
                {
                    _configuration = configuration;
                    string connectionString = _configuration["PublicFunction:DataBase:Oracle:OracleService:ConnectionString"];
                    _oracleConnection = new OdbcConnection(connectionString);
                }

                // Property to get or set the stored procedure name
                public string StoredProcedureName
                {
                    get => _oracleCommand?.CommandText;
                    set
                    {
                        _oracleCommand = new OdbcCommand(value, _oracleConnection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                    }
                }

                // Add a parameter with specified OdbcType
                public void AddParameter(string name, OdbcType dbType, object data)
                {
                    try
                    {
                        if (_oracleCommand == null)
                            throw new InvalidOperationException("StoredProcedureName must be set before adding parameters.");

                        var parameter = new OdbcParameter(name, dbType)
                        {
                            Value = data ?? DBNull.Value
                        };
                        _oracleCommand.Parameters.Add(parameter);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error adding parameter '{name}': {ex.Message}", ex);
                    }
                }

                // Add a parameter without specifying OdbcType (inferred from data)
                public void AddParameter(string name, object data)
                {
                    try
                    {
                        if (_oracleCommand == null)
                            throw new InvalidOperationException("StoredProcedureName must be set before adding parameters.");

                        var parameter = new OdbcParameter(name, data ?? DBNull.Value);
                        _oracleCommand.Parameters.Add(parameter);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error adding parameter '{name}': {ex.Message}", ex);
                    }
                }

                // Add parameters from a model that inherits from OracleStoredProcedureModel
                public void AddParametersFromModel<T>(T model) where T : OracleStoredProcedureModel
                {
                    try
                    {
                        if (_oracleCommand == null)
                            throw new InvalidOperationException("StoredProcedureName must be set before adding parameters.");

                        foreach (var prop in typeof(T).GetProperties())
                        {
                            var value = prop.GetValue(model);
                            var parameterName = ":" + prop.Name; // Oracle uses ":" as prefix for parameters
                            _oracleCommand.Parameters.Add(new OdbcParameter(parameterName, value ?? DBNull.Value));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error adding parameters from model: {ex.Message}", ex);
                    }
                }

                // Execute a non-query stored procedure (e.g., INSERT, UPDATE, DELETE)
                public bool Execute()
                {
                    try
                    {
                        _oracleCommand.Connection = _oracleConnection;
                        _oracleConnection.Open();
                        _oracleCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error executing stored procedure '{StoredProcedureName}': {ex.Message}", ex);
                    }
                    finally
                    {
                        _oracleConnection.Close();
                    }
                }

                // Execute a SELECT query and return a DataTable
                public DataTable Select()
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        _oracleCommand.Connection = _oracleConnection;
                        _oracleConnection.Open();
                        using (OdbcDataAdapter adapter = new OdbcDataAdapter(_oracleCommand))
                        {
                            adapter.Fill(dt);
                        }
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error executing select query: {ex.Message}", ex);
                    }
                    finally
                    {
                        _oracleConnection.Close();
                    }
                }

                // Execute a SELECT query and return a list of dictionaries
                public List<Dictionary<string, object>> SelectList()
                {
                    try
                    {
                        DataTable dt = Select();
                        var list = new List<Dictionary<string, object>>();

                        foreach (DataRow row in dt.Rows)
                        {
                            var dict = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                            }
                            list.Add(dict);
                        }

                        return list;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error converting select results to list: {ex.Message}", ex);
                    }
                }

                // Execute a SELECT query and map the first result to a model
                public T SelectModel<T>() where T : OracleDataModel, new()
                {
                    try
                    {
                        DataTable dt = Select();
                        if (dt.Rows.Count == 0)
                            return default;

                        T model = new T();
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            if (dt.Columns.Contains(prop.Name))
                            {
                                var value = dt.Rows[0][prop.Name];
                                prop.SetValue(model, value == DBNull.Value ? null : value);
                            }
                        }
                        return model;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error mapping select results to model: {ex.Message}", ex);
                    }
                }

                // Execute multiple SELECT queries and return a DataSet
                public DataSet MultiSelect()
                {
                    try
                    {
                        DataSet ds = new DataSet();
                        _oracleCommand.Connection = _oracleConnection;
                        _oracleConnection.Open();
                        using (OdbcDataAdapter adapter = new OdbcDataAdapter(_oracleCommand))
                        {
                            adapter.Fill(ds);
                        }
                        return ds;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error executing multi-select queries: {ex.Message}", ex);
                    }
                    finally
                    {
                        _oracleConnection.Close();
                    }
                }

                // Execute multiple SELECT queries and return a list of lists of dictionaries
                public List<List<Dictionary<string, object>>> MultiSelectList()
                {
                    try
                    {
                        DataSet ds = MultiSelect();
                        var list = new List<List<Dictionary<string, object>>>();

                        foreach (DataTable table in ds.Tables)
                        {
                            var tableList = new List<Dictionary<string, object>>();
                            foreach (DataRow row in table.Rows)
                            {
                                var dict = new Dictionary<string, object>();
                                foreach (DataColumn col in table.Columns)
                                {
                                    dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                                }
                                tableList.Add(dict);
                            }
                            list.Add(tableList);
                        }

                        return list;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error converting multi-select results to list: {ex.Message}", ex);
                    }
                }

                // Execute multiple SELECT queries and map the results to a model with multiple tables
                public T MultiSelectModel<T>(string[] tableNames) where T : OracleDataModel, new()
                {
                    try
                    {
                        DataSet ds = MultiSelect();
                        T model = new T();

                        foreach (var tableName in tableNames)
                        {
                            if (ds.Tables.Contains(tableName))
                            {
                                var table = ds.Tables[tableName];
                                foreach (var prop in typeof(T).GetProperties())
                                {
                                    if (table.Columns.Contains(prop.Name))
                                    {
                                        var value = table.Rows[0][prop.Name];
                                        prop.SetValue(model, value == DBNull.Value ? null : value);
                                    }
                                }
                            }
                        }

                        return model;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error mapping multi-select results to model: {ex.Message}", ex);
                    }
                }

                // Insert a record into the specified table using a model
                public bool Insert<T>(string tableName, T model) where T : OracleDataModel
                {
                    try
                    {
                        var properties = typeof(T).GetProperties();
                        var columns = string.Join(", ", properties.Select(p => p.Name));
                        var values = string.Join(", ", properties.Select(p => ":" + p.Name));
                        var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                        using (OdbcCommand cmd = new OdbcCommand(query, _oracleConnection))
                        {
                            foreach (var prop in properties)
                            {
                                cmd.Parameters.AddWithValue(":" + prop.Name, prop.GetValue(model) ?? DBNull.Value);
                            }

                            _oracleConnection.Open();
                            cmd.ExecuteNonQuery();
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error inserting into table '{tableName}': {ex.Message}", ex);
                    }
                    finally
                    {
                        _oracleConnection.Close();
                    }
                }

                // Update a record in the specified table using a model and ID
                public bool Update<T>(string tableName, string id, T model) where T : OracleDataModel
                {
                    try
                    {
                        var properties = typeof(T).GetProperties();
                        var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = :{p.Name}"));
                        var query = $"UPDATE {tableName} SET {setClause} WHERE ID = :ID";

                        using (OdbcCommand cmd = new OdbcCommand(query, _oracleConnection))
                        {
                            cmd.Parameters.AddWithValue(":ID", id);
                            foreach (var prop in properties)
                            {
                                cmd.Parameters.AddWithValue(":" + prop.Name, prop.GetValue(model) ?? DBNull.Value);
                            }

                            _oracleConnection.Open();
                            cmd.ExecuteNonQuery();
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error updating table '{tableName}' with ID '{id}': {ex.Message}", ex);
                    }
                    finally
                    {
                        _oracleConnection.Close();
                    }
                }

                // Delete a record from the specified table using ID
                public bool Delete(string tableName, string id)
                {
                    try
                    {
                        var query = $"DELETE FROM {tableName} WHERE ID = :ID";

                        using (OdbcCommand cmd = new OdbcCommand(query, _oracleConnection))
                        {
                            cmd.Parameters.AddWithValue(":ID", id);

                            _oracleConnection.Open();
                            cmd.ExecuteNonQuery();
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error deleting from table '{tableName}' with ID '{id}': {ex.Message}", ex);
                    }
                    finally
                    {
                        _oracleConnection.Close();
                    }
                }
            }

    }
}
