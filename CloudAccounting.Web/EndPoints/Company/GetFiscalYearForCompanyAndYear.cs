using CloudAccounting.Application.UseCases.Company.GetFiscalYearByCompanyAndYear;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.Web.EndPoints.Company
{
    public class GetFiscalYearForCompanyAndYear : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("companies/{companyCode:int}/{fiscalYear:int}", CreateNewFiscalYear)
                .Produces(404)
                .Produces<CompanyWithFiscalPeriodsDto>()
                .Produces(500);
        }

        public static async Task<IResult> CreateNewFiscalYear(int companyCode, int fiscalYear, ISender sender, ILogger<CreateCompany> logger)
        {
            GetFiscalYearByCompanyAndYearQuery query = new(companyCode, fiscalYear);
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