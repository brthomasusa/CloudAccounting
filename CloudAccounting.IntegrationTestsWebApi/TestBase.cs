using System.Text.Json;
using CloudAccounting.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace CloudAccounting.IntegrationTestsWebApi;

public abstract class TestBase : IDisposable
{
    protected readonly CloudAccountingContext? _dbContext;
    protected readonly JsonSerializerOptions? _options;
    protected readonly IMemoryCache? _memoryCache;

    protected TestBase()
    {
        try
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            string connectionString = "User Id=CLOUD_ACCTG_DEV;Password=Info33Gum;Data Source=rhel9-ws:1521/ORCL;";
            string storedProc = "BEGIN reset_db_to_known_state; END;";

            var optionsBuilder = new DbContextOptionsBuilder<CloudAccountingContext>();

            optionsBuilder.UseOracle(connectionString)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();

            _dbContext = new CloudAccountingContext(optionsBuilder.Options);
            int rowsAffected = _dbContext.Database.ExecuteSqlRaw(storedProc);

            // Setup IMemoryCache
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            _memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }
    }

    public void Dispose()
    {
        _dbContext!.Dispose();
        _memoryCache!.Dispose();

        GC.SuppressFinalize(this);
    }
}