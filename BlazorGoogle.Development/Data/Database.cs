using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace BlazorGoogle.Development.Data
{
    public class Database : IDatabase, IProcs
    {
        private SqlCommand m_Command;
        private readonly SqlConnection m_conn;
        private SqlDataReader m_Reader;

        // Properties
        public IDataReader Reader => m_Reader;
        public SqlCommand Command => m_Command;

        // Constructor
        public Database(IDatabaseSettings settings)
        {
            if (string.IsNullOrEmpty(settings?.ConnectionString))
                throw new ApplicationException("The connection string was empty");

            m_conn = new SqlConnection(settings.ConnectionString);
            m_Command = new SqlCommand { Connection = m_conn };
        }

        // SQL Commands

        public void RunSqlFile(string filePath)
        {
            CheckConnection();
            m_Command.CommandType = CommandType.Text;

            string script = File.ReadAllText(filePath);
            var reg = new Regex("^GO", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            string[] parts = reg.Split(script);

            foreach (var str in parts)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    m_Command.CommandText = str;
                    m_Command.ExecuteNonQuery();
                }
            }
        }

        public async Task RunSqlFileAsync(string filePath)
        {
            CheckConnection();
            m_Command.CommandType = CommandType.Text;

            string script = await File.ReadAllTextAsync(filePath);
            var reg = new Regex("^GO", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            string[] parts = reg.Split(script);

            foreach (var str in parts)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    m_Command.CommandText = str;
                    await m_Command.ExecuteNonQueryAsync();
                }
            }
        }

        public int RunSql(string sql)
        {
            CheckConnection();
            m_Command.CommandText = sql;
            m_Command.CommandType = CommandType.Text;

            try
            {
                return m_Command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + sql, e);
            }
        }

        public async Task<int> RunSqlAsync(string sql)
        {
            CheckConnection();
            m_Command.CommandText = sql;
            m_Command.CommandType = CommandType.Text;

            try
            {
                return await m_Command.ExecuteNonQueryAsync();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + sql, e);
            }
        }

        public void RunSqlReturnReader(string sql)
        {
            CheckConnection();
            m_Command.CommandText = sql;
            m_Command.CommandType = CommandType.Text;

            try
            {
                m_Reader = m_Command.ExecuteReader();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + sql, e);
            }
        }

        public async Task RunSqlReturnReaderAsync(string sql)
        {
            CheckConnection();
            m_Command.CommandText = sql;
            m_Command.CommandType = CommandType.Text;

            try
            {
                m_Reader = await m_Command.ExecuteReaderAsync();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + sql, e);
            }
        }

        public object RunSqlReturnScalar(string sql)
        {
            CheckConnection();
            m_Command.CommandText = sql;
            m_Command.CommandType = CommandType.Text;

            try
            {
                return m_Command.ExecuteScalar();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + sql, e);
            }
        }

        public async Task<object> RunSqlReturnScalarAsync(string sql)
        {
            CheckConnection();
            m_Command.CommandText = sql;
            m_Command.CommandType = CommandType.Text;

            try
            {
                return await m_Command.ExecuteScalarAsync();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + sql, e);
            }
        }

        public void RunSPReturnReader(string proc)
        {
            CheckConnection();
            m_Command.CommandText = proc;
            m_Command.CommandType = CommandType.StoredProcedure;

            try
            {
                m_Reader = m_Command.ExecuteReader();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + proc, e);
            }
        }

        public async Task RunSPReturnReaderAsync(string proc)
        {
            CheckConnection();
            m_Command.CommandText = proc;
            m_Command.CommandType = CommandType.StoredProcedure;

            try
            {
                m_Reader = await m_Command.ExecuteReaderAsync();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + proc, e);
            }
        }

        public int RunSP(string proc)
        {
            CheckConnection();
            m_Command.CommandText = proc;
            m_Command.CommandType = CommandType.StoredProcedure;

            try
            {
                return m_Command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + proc, e);
            }
        }

        public async Task<int> RunSPAsync(string proc)
        {
            CheckConnection();
            m_Command.CommandText = proc;
            m_Command.CommandType = CommandType.StoredProcedure;

            try
            {
                return await m_Command.ExecuteNonQueryAsync();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + proc, e);
            }
        }

        public object RunSPReturnScalar(string proc)
        {
            CheckConnection();
            m_Command.CommandText = proc;
            m_Command.CommandType = CommandType.StoredProcedure;

            try
            {
                return m_Command.ExecuteScalar();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + proc, e);
            }
        }

        public async Task<object> RunSPReturnScalarAsync(string proc)
        {
            CheckConnection();
            m_Command.CommandText = proc;
            m_Command.CommandType = CommandType.StoredProcedure;

            try
            {
                return await m_Command.ExecuteScalarAsync();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message + "----" + proc, e);
            }
        }

        /// <summary>
        /// Reset the reader and command objects and start again
        /// </summary>
        public void Refresh()
        {
            // Close and reopen the connection to ensure fresh data from the database.
            if (m_conn.State != ConnectionState.Closed)
                m_conn.Close();
            m_conn.Open();

            // Dispose the command and reader, then recreate them for a fresh state.
            m_Command?.Dispose();
            m_Reader?.Dispose();

            m_Command = new SqlCommand { Connection = m_conn };
            m_Reader = null;
        }


        // Miscellaneous Methods
        private void CheckConnection()
        {
            if (m_conn.State == ConnectionState.Closed)
                m_conn.Open();
        }

        public void Close()
        {
            m_Reader?.Close();
            m_Command?.Dispose();
            m_conn?.Close();
        }

        public DataTable GetDataTable()
        {
            return GetDataSet().Tables[0];
        }

        public DataRow GetDataRow()
        {
            return GetDataTable().Rows[0];
        }

        public DataSet GetDataSet()
        {
            if (m_Reader == null)
                throw new ApplicationException("The Data Reader is Not filled");

            var dataSet = new DataSet();
            do
            {
                var schemaTable = m_Reader.GetSchemaTable();
                var dataTable = new DataTable();

                if (schemaTable != null)
                {
                    foreach (DataRow dataRow in schemaTable.Rows)
                    {
                        string columnName = (string)dataRow["ColumnName"];
                        var column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    while (m_Reader.Read())
                    {
                        var dataRow = dataTable.NewRow();
                        for (int i = 0; i < m_Reader.FieldCount; i++)
                            dataRow[i] = m_Reader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    var column = new DataColumn("RowsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);
                    var dataRow = dataTable.NewRow();
                    dataRow[0] = m_Reader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            } while (m_Reader.NextResult());
            return dataSet;
        }

        public string GetJSON()
        {
            return DataTableToJSON(GetDataTable());
        }

        public string DataTableToJSON(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }

            return JsonSerializer.Serialize(list);
        }
    }
}
