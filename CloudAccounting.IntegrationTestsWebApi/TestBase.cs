namespace CloudAccounting.IntegrationTestsWebApi;

public abstract class TestBase : IDisposable
{
    protected readonly CloudAccountingContext? _efCoreContext;
    protected readonly DapperContext? _dapperContext;

    protected readonly JsonSerializerOptions? _options;
    protected readonly IMemoryCache? _memoryCache;

    protected TestBase()
    {
        try
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            string connectionString = "User Id=CLOUD_ACCTG_DEV;Password=Info33Gum;Data Source=rhel9-ws:1521/ORCL;";


            var optionsBuilder = new DbContextOptionsBuilder<CloudAccountingContext>();

            optionsBuilder.UseOracle(connectionString)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();

            _efCoreContext = new CloudAccountingContext(optionsBuilder.Options);
            _dapperContext = new(connectionString);

            string storedProc = "BEGIN reset_db_to_known_state; END;";
            int rowsAffected = _efCoreContext.Database.ExecuteSqlRaw(storedProc);

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
        _efCoreContext!.Dispose();
        _memoryCache!.Dispose();

        GC.SuppressFinalize(this);
    }
}