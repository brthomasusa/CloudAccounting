using CloudAccounting.Application.UseCases.GetCompanyById;
using CloudAccounting.Application.UseCases.GetAllCompanies;
using CloudAccounting.Application.ViewModels.Company;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyQueryHandlerTests : TestBase
{
    private readonly CompanyRepository _repository;
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

    public CompanyQueryHandlerTests()
    {
        _repository = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
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
        GetCompanyByIdQuery query = new(3);
        GetCompanyByIdQueryHandler handler = new(_repository, new NullLogger<GetCompanyByIdQueryHandler>(), _mapper);

        // Act
        Result<CompanyDetailVm> result = await handler.Handle(query, new CancellationToken());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("The company with CompanyCode '3' was not found.", result.Error.Message);
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
        Assert.Equal(2, result.Value.Count);
    }
}
