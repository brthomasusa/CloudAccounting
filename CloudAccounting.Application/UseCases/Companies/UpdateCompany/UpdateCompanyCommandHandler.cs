using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Companies.UpdateCompany;

public class UpdateCompanyCommandHandler
(
    ICompanyRepository repository,
    ILogger<UpdateCompanyCommandHandler> logger
) : ICommandHandler<UpdateCompanyCommand, CompanyDetailDto>
{
    private readonly ICompanyRepository _repository = repository;
    private readonly ILogger<UpdateCompanyCommandHandler> _logger = logger;

    public async Task<Result<CompanyDetailDto>> Handle(UpdateCompanyCommand command, CancellationToken token)
    {
        Company company = command.Adapt<Company>();
        Result<Company> updateCompanyResult = await _repository.UpdateAsync(company);

        if (updateCompanyResult.IsFailure)
        {
            return Result<CompanyDetailDto>.Failure<CompanyDetailDto>(
                new Error("UpdateCompanyCommandHandler.Handle", updateCompanyResult.Error.Message)
            );
        }

        CompanyDetailDto companyDetail = updateCompanyResult.Value.Adapt<CompanyDetailDto>();

        return companyDetail;
    }
}
