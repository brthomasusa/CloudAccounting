using CloudAccounting.Application.UseCases.Companies.GetAllCompanies;
using CloudAccounting.Application.UseCases.Companies.GetCompany;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.IntegrationTests.CompanyTests;

[Collection("SequentialTestCollection")]
public class CompanyQueryHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;
    private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;
    private ICompanyRepository _repo => new CompanyRepository(_context, _memoryCache!, new NullLogger<CompanyRepository>(), _mapper);
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();
    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task GetAllCompaniesQueryHandler_ReturnsMultibleCompanies()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 5;
        await ReseedTestDb.ReseedTestDbAsync(_context);
        GetAllCompaniesQueryHandler handler = new(_repo, new NullLogger<GetAllCompaniesQueryHandler>());
        GetAllCompaniesQuery query = new(pageNumber, pageSize);

        // Act
        Result<List<CompanyDetailDto>> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task Handle_GetCompanyByIdQueryHandler_ShouldReturn_1_CompanyDetailVm()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        GetCompanyByIdQuery query = new(1);
        GetCompanyByIdQueryHandler handler = new(_repo, new NullLogger<GetCompanyByIdQueryHandler>());

        // Act
        Result<CompanyDetailDto> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("BTechnical Consulting", result.Value.CompanyName);
    }

    [Fact]
    public async Task Handle_GetCompanyByIdQueryHandler__InvalidCompanyCode_ShouldReturnFailure()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        GetCompanyByIdQuery query = new(-3);
        GetCompanyByIdQueryHandler handler = new(_repo, new NullLogger<GetCompanyByIdQueryHandler>());

        // Act
        Result<CompanyDetailDto> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("The company with CompanyCode '-3' was not found.", result.Error.Message);
    }

}
