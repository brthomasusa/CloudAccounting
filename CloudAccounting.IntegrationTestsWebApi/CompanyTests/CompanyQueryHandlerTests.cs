using CloudAccounting.Application.UseCases.GetCompanyById;
using CloudAccounting.Application.UseCases.GetAllCompanies;
using CloudAccounting.Application.UseCases.Company.GetFiscalYearByCompanyAndYear;
using CloudAccounting.Application.UseCases.Company.GetCompanyWithNoFiscalYearData;
using CloudAccounting.Application.UseCases.Company.GetNextValidFiscalYearStart;
using CloudAccounting.Application.UseCases.Lookups.CompanyCodeLookup;
using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyQueryHandlerTests : TestBase
{
    private readonly CompanyRepository _repository;
    private readonly CompanyReadRepository _readRepository;
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

    public CompanyQueryHandlerTests()
    {
        _repository = new(_efCoreContext!, _memoryCache!, new NullLogger<CompanyRepository>(), _dapperContext!, _mapper);
        _readRepository = new(_dapperContext!, new NullLogger<CompanyReadRepository>());
    }

    [Fact]
    public async Task Handle_GetCompanyByIdQueryHandler_ShouldReturn_1_CompanyDetailVm()
    {
        // Arrange
        GetCompanyByIdQuery query = new(1);
        GetCompanyByIdQueryHandler handler = new(_repository, new NullLogger<GetCompanyByIdQueryHandler>(), _mapper);

        // Act
        Result<CompanyDetailVm> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("BTechnical Consulting", result.Value.CompanyName);
    }

    [Fact]
    public async Task Handle_GetCompanyByIdQueryHandler__InvalidCompanyCode_ShouldReturnFailure()
    {
        // Arrange
        GetCompanyByIdQuery query = new(-3);
        GetCompanyByIdQueryHandler handler = new(_repository, new NullLogger<GetCompanyByIdQueryHandler>(), _mapper);

        // Act
        Result<CompanyDetailVm> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("The company with CompanyCode '-3' was not found.", result.Error.Message);
    }

    [Fact]
    public async Task Handle_GetAllCompaniesQueryHandler_ShouldReturn_2_CompanyDetailVm()
    {
        // Arrange
        GetAllCompaniesQuery query = new(1, 5);
        GetAllCompaniesQueryHandler handler = new(_repository, new NullLogger<GetAllCompaniesQueryHandler>(), _mapper);

        // Act
        Result<List<CompanyDetailVm>> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Count > 1);
    }

    [Fact]
    public async Task Handle_GetFiscalYearByCompanyAndYearQueryHandler_ShouldReturn_1_CompanyWithFiscalPeriodsDto()
    {
        // Arrange
        GetFiscalYearByCompanyAndYearQuery query = new(4, 2024);
        GetFiscalYearByCompanyAndYearQueryHandler handler = new(_readRepository, new NullLogger<GetFiscalYearByCompanyAndYearQueryHandler>());

        // Act
        Result<CompanyWithFiscalPeriodsDto> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Maulibu Bar & Grill", result.Value.CompanyName);
        Assert.True(result.Value.IsInitialYear);
        Assert.Equal(12, result.Value.FiscalPeriods.Count);
    }

    [Fact]
    public async Task Handle_GetCompanyWithNoFiscalYearDataQueryHandler_ShouldReturn_1_CompanyWithoutFiscalPeriodsDto()
    {
        // Arrange
        GetCompanyWithNoFiscalYearDataQuery query = new(3);
        GetCompanyWithNoFiscalYearDataQueryHandler handler = new(_readRepository, new NullLogger<GetCompanyWithNoFiscalYearDataQueryHandler>());

        // Act
        Result<CompanyWithFiscalPeriodsDto> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Rooms-2-Go", result.Value.CompanyName);
        ; Assert.Empty(result.Value.FiscalPeriods);
    }

    [Fact]
    public async Task Handle_CompanyCodeLookupQueryHandler_ShouldReturn_List_CompanyLookups()
    {
        // Arrange
        CompanyCodeLookupQuery query = new();
        CompanyCodeLookupQueryHandler handler = new(_readRepository, new NullLogger<CompanyCodeLookupQueryHandler>());

        // Act
        Result<List<CompanyLookup>> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task Handle_GetNextValidFiscalYearStartDateQueryHandler_ShouldReturn_NoneNullDateTime()
    {
        // Arrange
        GetNextValidFiscalYearStartDateQuery query = new(4);
        GetNextValidFiscalYearStartDateQueryHandler handler = new(_readRepository, new NullLogger<GetNextValidFiscalYearStartDateQueryHandler>());

        // Act
        Result<DateTime> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new DateTime(2026, 7, 1), result.Value);
    }

    [Fact]
    public async Task Handle_GetNextValidFiscalYearStartDateQueryHandler_ShouldReturn_NullDateTime()
    {
        // This test is for companies that have no fiscal year info.

        // Arrange
        GetNextValidFiscalYearStartDateQuery query = new(2);
        GetNextValidFiscalYearStartDateQueryHandler handler = new(_readRepository, new NullLogger<GetNextValidFiscalYearStartDateQueryHandler>());

        // Act
        Result<DateTime> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(DateTime.MinValue, result.Value);
    }
}
