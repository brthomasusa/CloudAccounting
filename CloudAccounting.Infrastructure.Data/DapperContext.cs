using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace CloudAccounting.Infrastructure.Data
{
    public class DapperContext(string connectionStr)
    {
        private readonly string _connectionStr = connectionStr;

        public OracleConnection CreateConnection() => new OracleConnection(_connectionStr);
    }
}