using CloudAccounting.Application.UseCases.Company.GetCompanyWithNoFiscalYearData;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.Web.EndPoints.Company
{
    public class GetCompanyWithoutFiscalInfo : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("companies/fiscalyear/{companyCode:int}", GetCompanyWithoutFiscalYear)
                .Produces(404)
                .Produces<CompanyWithFiscalPeriodsDto>()
                .Produces(500);
        }

        public static async Task<IResult> GetCompanyWithoutFiscalYear(int companyCode, ISender sender, ILogger<CreateCompany> logger)
        {
            GetCompanyWithNoFiscalYearDataQuery query = new(companyCode);
            Result<CompanyWithFiscalPeriodsDto>? result = null;

            try
            {
                result = await sender.Send(query);

                if (result.IsSuccess)
                {
                    return TypedResults.Ok(result.Value);
                }

                logger.LogWarning("There was a problem retrieving fiscal year information: {ERROR}", result.Error.Message);
                return result!.ToNotFoundProblemDetails();

            }
            catch (Exception ex)
            {
                string msg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", msg);

                return result!.ToInternalServerErrorProblemDetails(msg);
            }
        }
    }
}