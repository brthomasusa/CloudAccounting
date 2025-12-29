using Oracle.ManagedDataAccess.Client;

namespace CloudAccounting.Infrastructure.Data;

public class DapperOracleContext
{
    public DapperOracleContext(string connectionStr)
    {
        OracleConnection oracleConnection = new(connectionStr);
        oracleConnection.Open();
        GetOracleConnection = oracleConnection;
    }

    public OracleConnection GetOracleConnection { get; }

}