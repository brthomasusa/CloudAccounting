using CloudAccounting.Application.ViewModels.Company;
using CompanyDomainModel = CloudAccounting.Core.Models.Company;

namespace CloudAccounting.Application.UseCases.CreateCompany;

public class CreateCompanyCommandHandler
(
    ICompanyRepository repository,
    ILogger<CreateCompanyCommandHandler> logger,
    IMapper mapper
) : ICommandHandler<CreateCompanyCommand, CompanyDetailVm>
{
    private readonly ICompanyRepository _repository = repository;
    private readonly ILogger<CreateCompanyCommandHandler> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CompanyDetailVm>> Handle(CreateCompanyCommand command, CancellationToken token)
    {
        CompanyDomainModel company = _mapper.Map<CompanyDomainModel>(command);
        Result<CompanyDomainModel> createCompanyResult = await _repository.CreateAsync(company);

        CompanyDetailVm companyDetail = _mapper.Map<CompanyDetailVm>(createCompanyResult.Value);

        if (createCompanyResult.IsFailure)
        {
            return Result<CompanyDetailVm>.Failure<CompanyDetailVm>(
                new Error("CreateCommandHandler.Handle", createCompanyResult.Error.Message)
            );
        }

        return companyDetail;
    }
}
