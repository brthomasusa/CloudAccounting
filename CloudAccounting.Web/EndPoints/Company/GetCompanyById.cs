using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Application.UseCases.GetCompanyById;

namespace CloudAccounting.Web.EndPoints.Company;

public class GetCompanyById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("companies/{companyCode:int}", GetCompanyByCompanyCode).WithName("GetCompanyByCompanyCode");
    }

    public static async Task<IResult> GetCompanyByCompanyCode(int companyCode, ISender sender, ILogger<GetCompanyById> logger)
    {
        Result<CompanyDetailVm>? result = null;

        try
        {
            result = await sender.Send(new GetCompanyByIdQuery(CompanyCode: companyCode));

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem retrieving the company with code {CODE}: {ERROR}", companyCode, result.Error.Message);
            return Results.NotFound(msg);

        }
        catch (Exception ex)
        {
            string msg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", msg);
            return result!.ToInternalServerErrorProblemDetails(msg);
        }
    }
}
