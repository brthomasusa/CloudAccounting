using CloudAccounting.Application.UseCases.GetAllCompanies;
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

}
