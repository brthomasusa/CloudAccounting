using CloudAccounting.Application.ViewModels.Company;

namespace CloudAccounting.Application.UseCases.UpdateCompany
{
    public class UpdateCompanyCommandHandler
    (
    ICompanyRepository repository,
    ILogger<UpdateCompanyCommandHandler> logger,
    IMapper mapper
    ) : ICommandHandler<UpdateCompanyCommand, CompanyDetailVm>
    {
        private readonly ICompanyRepository _repository = repository;
        private readonly ILogger<UpdateCompanyCommandHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CompanyDetailVm>> Handle(UpdateCompanyCommand command, CancellationToken token)
        {
            Company company = _mapper.Map<Company>(command);
            Result<Company> updateCompanyResult = await _repository.UpdateAsync(company);

            if (updateCompanyResult.IsFailure)
            {
                return Result<CompanyDetailVm>.Failure<CompanyDetailVm>(
                    new Error("UpdateCompanyCommandHandler.Handle", updateCompanyResult.Error.Message)
                );
            }

            CompanyDetailVm companyDetail = _mapper.Map<CompanyDetailVm>(updateCompanyResult.Value);

            return companyDetail;
        }
    }
}