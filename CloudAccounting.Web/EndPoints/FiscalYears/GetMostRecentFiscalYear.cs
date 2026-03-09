using CloudAccounting.Application.UseCases.FiscalYears.GetMostRecentFiscalYear;
using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.Web.EndPoints.FiscalYears
{
    public class GetMostRecentFiscalYear : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("fiscalyears/{companyCode:int}", RetrieveFiscalYear)
                .Produces(404)
                .Produces<FiscalYearDto>()
                .Produces(500);
        }

        public static async Task<IResult> RetrieveFiscalYear(int companyCode, ISender sender, ILogger<GetMostRecentFiscalYear> logger)
        {
            GetMostRecentFiscalYearQuery query = new(companyCode);
            Result<FiscalYearDto>? result = await sender.Send(query);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            logger.LogWarning("There was a problem retrieving the most recentfiscal year information: {ERROR}", result.Error.Message);
            return result!.ToNotFoundProblemDetails();
        }

    }
}