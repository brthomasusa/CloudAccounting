namespace CloudAccounting.IntegrationTests;

public class DatabaseFixture : IAsyncLifetime
{
    public AppDbContext? Context { get; private set; }
    public IMemoryCache? MemoryCache { get; private set; }
    private Respawner? _respawner;
    private DbConnection? _connection;

    public async Task InitializeAsync()
    {
        // Setup IMemoryCache
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        MemoryCache = serviceProvider.GetRequiredService<IMemoryCache>();

        string connectionString = "Server=tcp:rhel9-ws,1433;Database=CloudAcctgAuth;User Id=sa;Password=Info99Gum;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        Context = new AppDbContext(options);

        var respawnerOptions = new RespawnerOptions
        {
            SchemasToInclude = ["dbo"],
            DbAdapter = DbAdapter.SqlServer
        };

        _connection = new SqlConnection(connectionString);
        await _connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_connection, respawnerOptions);
    }

    public Task ResetDatabase()
    {
        return _respawner!.ResetAsync(_connection!);
    }

    public async Task DisposeAsync()
    {
        if (Context != null)
        {
            await Context.DisposeAsync();
        }
    }
}
