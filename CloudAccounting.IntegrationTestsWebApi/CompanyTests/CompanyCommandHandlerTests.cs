using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Application.UseCases.UpdateCompany;
using CloudAccounting.Application.UseCases.DeleteCompany;
using CloudAccounting.Application.ViewModels.Company;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyCommandHandlerTests : TestBase
{
    private readonly CompanyRepository _repository;
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

    public CompanyCommandHandlerTests()
    {
        _repository = new(_efCoreContext!, _memoryCache!, new NullLogger<CompanyRepository>(), _dapperContext!, _mapper);
    }

    [Fact]
    public async Task Handle_CreateCompanyCommandHandler_GivenValidCmd_ShouldSucceed()
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

    [Fact]
    public async Task Handle_UpdateCompanyCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        UpdateCompanyCommandHandler handler = new(_repository, new NullLogger<UpdateCompanyCommandHandler>(), _mapper);
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
        DeleteCompanyCommandHandler handler = new(_repository, new NullLogger<DeleteCompanyCommandHandler>());
        DeleteCompanyCommand command = new() { CompanyCode = 2 };

        // Act
        Result<MediatR.Unit> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
    }

}
