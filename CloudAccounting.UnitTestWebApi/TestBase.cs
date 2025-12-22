using CloudAccounting.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CloudAccounting.UnitTestWebApi
{
    public abstract class TestBase : IDisposable
    {
        protected readonly CloudAccountingContext? _dbContext;

        protected TestBase()
        {
            try
            {
                string connectionString = "User Id=CLOUD_ACCTG_DEV;Password=Info33Gum;Data Source=rhel9-ws:1521/ORCL;";
                string storedProc = "BEGIN reset_db_to_known_state; END;";

                var optionsBuilder = new DbContextOptionsBuilder<CloudAccountingContext>();

                optionsBuilder.UseOracle(connectionString)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

                _dbContext = new CloudAccountingContext(optionsBuilder.Options);
                int rowsAffected = _dbContext.Database.ExecuteSqlRaw(storedProc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        public void Dispose()
        {
            _dbContext!.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}