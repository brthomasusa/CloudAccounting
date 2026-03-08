using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Companies.GetAllCompanies;

public class GetAllCompaniesQueryHandler
(
    ICompanyRepository repository,
    ILogger<GetAllCompaniesQueryHandler> logger
) : IQueryHandler<GetAllCompaniesQuery, List<CompanyDetailDto>>
{

    private readonly ICompanyRepository _repository = repository;
    private readonly ILogger<GetAllCompaniesQueryHandler> _logger = logger;

    public async Task<Result<List<CompanyDetailDto>>> Handle
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
                List<CompanyDetailDto> companyDetailVms = [];

                foreach (Company company in getAllCompaniesResult.Value)
                {
                    companyDetailVms.Add(company.Adapt<CompanyDetailDto>());
                }

                return companyDetailVms;
            }

            return Result<List<CompanyDetailDto>>.Failure<List<CompanyDetailDto>>(
                new Error("GetAllCompaniesQueryHandler.Handle", getAllCompaniesResult.Error.Message)
            );

        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            _logger.LogError(ex, "{Message}", errMsg);

            return Result<List<CompanyDetailDto>>.Failure<List<CompanyDetailDto>>(
                new Error("GetAllCompaniesQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }
}
