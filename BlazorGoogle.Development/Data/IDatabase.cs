using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGoogle.Development.Data
{
    public interface IDatabase
    {
        void Refresh();

        void Close();

        void RunSqlFile(String FilePath);

        Task RunSqlFileAsync(String FilePath);

        int RunSql(string sql);

        Task<int> RunSqlAsync(string sql);

        void RunSqlReturnReader(string sql);

        Task RunSqlReturnReaderAsync(string sql);

        object RunSqlReturnScalar(string sql);

        Task<object> RunSqlReturnScalarAsync(string sql);

        DataTable GetDataTable();

        DataRow GetDataRow();

        DataSet GetDataSet();
        string DataTableToJSON(DataTable table);
    }
}
