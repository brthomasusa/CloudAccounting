using CloudAccounting.Application.ViewModels.Company;
using CompanyDomainModel = CloudAccounting.Core.Models.Company;

namespace CloudAccounting.Application.UseCases.GetCompanyById;

public class GetCompanyByIdQueryHandler
(
    ICompanyRepository repository,
    ILogger<GetCompanyByIdQueryHandler> logger,
    IMapper mapper
) : IQueryHandler<GetCompanyByIdQuery, CompanyDetailVm>
{
    private readonly ICompanyRepository _repository = repository;
    private readonly ILogger<GetCompanyByIdQueryHandler> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CompanyDetailVm>> Handle
    (
        GetCompanyByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Result<CompanyDomainModel> result = await _repository.RetrieveAsync(query.CompanyCode, new CancellationToken());

            if (result.IsFailure)
            {
                return Result<CompanyDetailVm>.Failure<CompanyDetailVm>(
                    new Error("GetCompanyByIdQueryHandler.Handle", result.Error.Message)
                );
            }

            CompanyDetailVm companyDetailVm = _mapper.Map<CompanyDetailVm>(result.Value);
            return companyDetailVm;
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            _logger.LogError(ex, "{Message}", errMsg);

            return Result<CompanyDetailVm>.Failure<CompanyDetailVm>(
                new Error("GetCompanyByIdQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }
}
