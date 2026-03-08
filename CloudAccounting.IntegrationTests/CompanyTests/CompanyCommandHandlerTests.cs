using CloudAccounting.Application.UseCases.Companies.UpdateCompany;
using CloudAccounting.Application.UseCases.Companies.CreateCompany;
using CloudAccounting.Application.UseCases.Companies.DeleteCompany;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.IntegrationTests.CompanyTests;

[Collection("SequentialTestCollection")]
public class CompanyCommandHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly CloudAccountingContext _context = fixture.Context!;
    private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;
    private ICompanyRepository _repo => new CompanyRepository(_context, _memoryCache!, new NullLogger<CompanyRepository>(), _mapper);
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();
    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task Handle_CreateCompanyCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateCompanyCommandHandler handler = new(_repo, new NullLogger<CreateCompanyCommandHandler>());
        CreateCompanyCommand command = TestData.CompanyTestData.GetCreateCompanyCommand();

        // Act
        Result<CompanyDetailDto> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.CompanyName, result.Value.CompanyName);
    }

    [Fact]
    public async Task Handle_UpdateCompanyCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        UpdateCompanyCommandHandler handler = new(_repo, new NullLogger<UpdateCompanyCommandHandler>());
        UpdateCompanyCommand command = TestData.CompanyTestData.GetUpdateCompanyCommand();

        // Act
        Result<CompanyDetailDto> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.Address, result.Value.Address);
    }

    [Fact]
    public async Task Handle_DeleteCompanyCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        DeleteCompanyCommandHandler handler = new(_repo, new NullLogger<DeleteCompanyCommandHandler>());
        DeleteCompanyCommand command = new() { CompanyCode = 2 };

        // Act
        Result<MediatR.Unit> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_DeleteCompanyCommandHandler_ShouldFail_WhenCompanyHasFiscalYears()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        DeleteCompanyCommandHandler handler = new(_repo, new NullLogger<DeleteCompanyCommandHandler>());
        DeleteCompanyCommand command = new() { CompanyCode = 1 };

        // Act
        Result<MediatR.Unit> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsFailure);
    }
}
