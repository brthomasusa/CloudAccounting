using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Application.ViewModels.Company;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyCommandHandlerTests : TestBase
{
    private readonly CompanyRepository _repository;
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

    public CompanyCommandHandlerTests()
    {
        _repository = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
    }

    [Fact]
    public async Task Handle_CreateCompanyCommandHandler_GivenValidCmd_ShouldReturnNewCompanyCode()
    {
        // Arrange
        CreateCompanyCommandHandler handler = new(_repository, new NullLogger<CreateCompanyCommandHandler>(), _mapper);
        CreateCompanyCommand command = TestData.CompanyTestData.GetCreateCompanyCommand();

        // Act
        Result<int> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value > 2);
    }

}
