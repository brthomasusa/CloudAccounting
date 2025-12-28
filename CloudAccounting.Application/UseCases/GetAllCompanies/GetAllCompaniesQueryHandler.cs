using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Core.Models;

namespace CloudAccounting.Application.UseCases.GetAllCompanies;

public class GetAllCompaniesQueryHandler
(
    ICompanyRepository repository,
    ILogger<GetAllCompaniesQueryHandler> logger,
    IMapper mapper
) : IQueryHandler<GetAllCompaniesQuery, List<CompanyDetailVm>>
{

    private readonly ICompanyRepository _repository = repository;
    private readonly ILogger<GetAllCompaniesQueryHandler> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<CompanyDetailVm>>> Handle
    (
        GetAllCompaniesQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Result<List<Company>> getAllCompaniesResult =
                await _repository.RetrieveAllAsync(query.PageNumber, query.PageSize);

            if (getAllCompaniesResult.IsSuccess)
            {
                List<CompanyDetailVm> companyDetailVms = [];

                foreach (Company company in getAllCompaniesResult.Value)
                {
                    companyDetailVms.Add(_mapper.Map<CompanyDetailVm>(company));
                }

                return companyDetailVms;
            }

            return Result<List<CompanyDetailVm>>.Failure<List<CompanyDetailVm>>(
                new Error("GetAllCompaniesQueryHandler.Handle", getAllCompaniesResult.Error.Message)
            );

        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            _logger.LogError(ex, "{Message}", errMsg);

            return Result<List<CompanyDetailVm>>.Failure<List<CompanyDetailVm>>(
                new Error("GetAllCompaniesQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }



}
