using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Application.ViewModels.Company;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyCommandHandlerTests : TestBase
{
    private readonly CompanyRepository _repository;
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

    public CompanyCommandHandlerTests()
    {
        _repository = new(_efCoreContext!, _memoryCache!, new NullLogger<CompanyRepository>(), _dapperContext!);
    }

    [Fact]
    public async Task Handle_CreateCompanyCommandHandler_GivenValidCmd_ShouldReturnNewCompanyCode()
    {
        // Arrange
        CreateCompanyCommandHandler handler = new(_repository, new NullLogger<CreateCompanyCommandHandler>(), _mapper);
        CreateCompanyCommand command = TestData.CompanyTestData.GetCreateCompanyCommand();

        // Act
        Result<CompanyDetailVm> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.CompanyCode > 2);
        Assert.Equal(command.CompanyName, result.Value.CompanyName);
    }

}
