using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Companies.CreateCompany;

public class CreateCompanyCommandHandler
(
    ICompanyRepository repository,
    ILogger<CreateCompanyCommandHandler> logger
) : ICommandHandler<CreateCompanyCommand, CompanyDetailDto>
{
    private readonly ICompanyRepository _repository = repository;
    private readonly ILogger<CreateCompanyCommandHandler> _logger = logger;

    public async Task<Result<CompanyDetailDto>> Handle(CreateCompanyCommand command, CancellationToken token)
    {
        Company company = command.Adapt<Company>();
        Result<Company> createCompanyResult = await _repository.CreateAsync(company);

        if (createCompanyResult.IsFailure)
        {
            return Result<CompanyDetailDto>.Failure<CompanyDetailDto>(
                new Error("CreateCommandHandler.Handle", createCompanyResult.Error.Message)
            );
        }

        CompanyDetailDto companyDetail = createCompanyResult.Value.Adapt<CompanyDetailDto>();

        return companyDetail;
    }
}
