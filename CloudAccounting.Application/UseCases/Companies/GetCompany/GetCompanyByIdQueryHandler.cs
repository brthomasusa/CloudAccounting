using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Companies.GetCompany;

public class GetCompanyByIdQueryHandler
(
    ICompanyRepository repository,
    ILogger<GetCompanyByIdQueryHandler> logger
) : IQueryHandler<GetCompanyByIdQuery, CompanyDetailDto>
{
    private readonly ICompanyRepository _repository = repository;
    private readonly ILogger<GetCompanyByIdQueryHandler> _logger = logger;

    public async Task<Result<CompanyDetailDto>> Handle
    (
        GetCompanyByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Result<Company> result = await _repository.RetrieveAsync(query.CompanyCode, new CancellationToken());

            if (result.IsFailure)
            {
                return Result<CompanyDetailDto>.Failure<CompanyDetailDto>(
                    new Error("GetCompanyByIdQueryHandler.Handle", result.Error.Message)
                );
            }

            CompanyDetailDto companyDetailDto = result.Value.Adapt<CompanyDetailDto>();

            return companyDetailDto;
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            _logger.LogError(ex, "{Message}", errMsg);

            return Result<CompanyDetailDto>.Failure<CompanyDetailDto>(
                new Error("GetCompanyByIdQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }
}
