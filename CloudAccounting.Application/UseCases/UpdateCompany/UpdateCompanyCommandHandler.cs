using CloudAccounting.Application.ViewModels.Company;
using CompanyDomainModel = CloudAccounting.Core.Models.Company;

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
            CompanyDomainModel company = _mapper.Map<CompanyDomainModel>(command);
            Result<CompanyDomainModel> updateCompanyResult = await _repository.UpdateAsync(company);

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