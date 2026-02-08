using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Application.UseCases.GetAllCompanies;

namespace CloudAccounting.Web.EndPoints.Company;

public class GetAllCompanies : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("companies", GetAllCompaniesWithPagingInfo);
    }

    public static async Task<IResult> GetAllCompaniesWithPagingInfo
    (
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        ISender sender,
        ILogger<GetAllCompanies> logger
    )
    {
        Result<List<CompanyDetailVm>> result =
            await sender.Send(new GetAllCompaniesQuery(PageNumber: pageNumber, PageSize: pageSize));

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        logger.LogWarning("There was problem retrieving the list of companies: {Message}", result.Error.Message);
        return result!.ToInternalServerErrorProblemDetails(result.Error.Message);
    }

}
