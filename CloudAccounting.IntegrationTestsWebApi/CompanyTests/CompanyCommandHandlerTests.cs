using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Application.UseCases.UpdateCompany;
using CloudAccounting.Application.UseCases.DeleteCompany;
using CloudAccounting.Application.UseCases.Company.CreateFiscalYear;
using CloudAccounting.Application.UseCases.Company.DeleteFiscalYear;
using CloudAccounting.Application.Services;
using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyCommandHandlerTests : TestBase
{
    private readonly CompanyRepository _companyRepo;
    private readonly FiscalYearRepository _fiscalYearRepo;
    private readonly CompanyReadRepository _readRepository;
    private readonly CompanyService _companyService;
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

    public CompanyCommandHandlerTests()
    {
        _readRepository = new(_dapperContext!, new NullLogger<CompanyReadRepository>());
        _companyRepo = new(_efCoreContext!, _memoryCache!, new NullLogger<CompanyRepository>(), _dapperContext!, _mapper);
        _fiscalYearRepo = new(_efCoreContext!, _memoryCache!, new NullLogger<FiscalYearRepository>(), _dapperContext!, _mapper);
        _companyService = new(_companyRepo, _readRepository);
    }

    [Fact]
    public async Task Handle_CreateCompanyCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        CreateCompanyCommandHandler handler = new(_companyRepo, new NullLogger<CreateCompanyCommandHandler>(), _mapper);
        CreateCompanyCommand command = TestData.CompanyTestData.GetCreateCompanyCommand();

        // Act
        Result<CompanyDetailVm> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.CompanyCode > 2);
        Assert.Equal(command.CompanyName, result.Value.CompanyName);
    }

    [Fact]
    public async Task Handle_UpdateCompanyCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        UpdateCompanyCommandHandler handler = new(_companyRepo, new NullLogger<UpdateCompanyCommandHandler>(), _mapper);
        UpdateCompanyCommand command = TestData.CompanyTestData.GetUpdateCompanyCommand();

        // Act
        Result<CompanyDetailVm> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.Address, result.Value.Address);
    }

    [Fact]
    public async Task Handle_DeleteCompanyCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        DeleteCompanyCommandHandler handler = new(_companyRepo, new NullLogger<DeleteCompanyCommandHandler>());
        DeleteCompanyCommand command = new() { CompanyCode = 2 };

        // Act
        Result<MediatR.Unit> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_CreateFiscalYearCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        CreateFiscalYearCommandHandler handler = new(_fiscalYearRepo, _companyService, new NullLogger<CreateFiscalYearCommandHandler>(), _mapper);
        CreateFiscalYearCommand command = new(2, 2025, 3);


        // Act
        Result<CompanyWithFiscalPeriodsDto> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);

    }

    [Fact]
    public async Task Handle_DeleteFiscalYearCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        DeleteFiscalYearCommandHandler handler = new(_companyRepo, new NullLogger<DeleteFiscalYearCommandHandler>());
        DeleteFiscalYearCommand command = new(4, 2025);

        // Act
        Result<MediatR.Unit> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);

    }


}
