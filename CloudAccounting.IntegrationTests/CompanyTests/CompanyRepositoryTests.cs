namespace CloudAccounting.IntegrationTests.CompanyTests;

[Collection("SequentialTestCollection")]
public class CompanyRepositoryTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly CloudAccountingContext _context = fixture.Context!;
    private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;
    private ICompanyRepository _repo => new CompanyRepository(_context, _memoryCache!, new NullLogger<CompanyRepository>(), _mapper);
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();
    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task RetrieveAllAsync_CompanyRepository_ReturnsMultibleCompanies()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 5;
        await ReseedTestDb.ReseedTestDbAsync(_context);

        // Act
        Result<List<Company>> result = await _repo.RetrieveAllAsync(pageNumber, pageSize);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task RetrieveAsync_CompanyRepository_ReturnsOneCompany()
    {
        // Arrange
        int companyCode = 1;
        await ReseedTestDb.ReseedTestDbAsync(_context);

        // Act
        Result<Company> result = await _repo.RetrieveAsync(companyCode, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("BTechnical Consulting", result.Value.CompanyName);
    }

    [Fact]
    public async Task Create_CompanyRepository_CreatesAndReturnsOneCompany()
    {
        // Arrange
        Company companyToCreate = CompanyTestData.GetCompanyForCreate();

        // Act
        Result<Company> result = await _repo.CreateAsync(companyToCreate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(companyToCreate.CompanyName, result.Value.CompanyName);
    }

    [Fact]
    public async Task Update_CompanyRepository_UpdatesAndReturnsOneCompany()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        Company companyToUpdate = CompanyTestData.GetCompanyForUpdate();

        // Act
        Result<Company> result = await _repo.UpdateAsync(companyToUpdate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(companyToUpdate.CompanyName, result.Value.CompanyName);
    }

    [Fact]
    public async Task Delete_CompanyRepository_DeletesOneCompanyAndReturnsTrue()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 2;

        // Act
        Result result = await _repo.DeleteAsync(companyCode);
        Result<Company> deletedCompanyResult = await _repo.RetrieveAsync(companyCode, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(deletedCompanyResult.IsFailure);
    }

    [Fact]
    public async Task IsUniqueCompanyNameForCreate_CompanyRepository_ShouldReturnsTrue()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        string companyName = "Hello World";

        // Act
        Result<bool> result = await _repo.IsUniqueCompanyNameForCreate(companyName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task IsUniqueCompanyNameForCreate_CompanyRepository_ShouldReturnsFalse()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        string companyName = "BTechnical Consulting";

        // Act
        Result<bool> result = await _repo.IsUniqueCompanyNameForCreate(companyName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
    }

    [Fact]
    public async Task IsUniqueCompanyNameForUpdate_CompanyRepository_ShouldReturnsTrue_NameChange()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;
        string companyName = "New World Importers";   // An update is being performed which changes the company name

        // Act
        Result<bool> result = await _repo.IsUniqueCompanyNameForUpdate(companyCode, companyName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task IsUniqueCompanyNameForUpdate_CompanyRepository_ShouldReturnsTrue_NoNameChange()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;
        string companyName = "BTechnical Consulting";   // An update is being perform which does not involve changing the company name

        // Act
        Result<bool> result = await _repo.IsUniqueCompanyNameForUpdate(companyCode, companyName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task IsUniqueCompanyNameForUpdate_CompanyRepository_ShouldReturnsFalse_DupeName()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;
        string companyName = "Contoso Ltd.";  // Another company already has this name.

        // Act
        Result<bool> result = await _repo.IsUniqueCompanyNameForUpdate(companyCode, companyName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
    }

    [Fact]
    public async Task IsExistingCompany_CompanyRepository_ShouldReturnsTrue()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;

        // Act
        Result<bool> result = await _repo.IsExistingCompany(companyCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task IsExistingCompany_CompanyRepository_ShouldReturnsFalse()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 13;

        // Act
        Result<bool> result = await _repo.IsExistingCompany(companyCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
    }
}
