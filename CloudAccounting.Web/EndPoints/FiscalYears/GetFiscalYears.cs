using CloudAccounting.Shared.FiscalYear;
using CloudAccounting.Application.UseCases.FiscalYears.GetFiscalYear;

namespace CloudAccounting.Web.EndPoints.FiscalYears
{
    public class GetFiscalYears : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("fiscalyears/{companyCode:int}/{fiscalYear:int}", RetrieveFiscalYear)
                .Produces(404)
                .Produces<FiscalYearDto>()
                .Produces(500);
        }

        public static async Task<IResult> RetrieveFiscalYear(int companyCode, int fiscalYear, ISender sender, ILogger<GetFiscalYears> logger)
        {
            GetFiscalYearQuery query = new(companyCode, fiscalYear);
            Result<FiscalYearDto>? result = await sender.Send(query);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            logger.LogWarning("There was a problem retrieving fiscal year information: {ERROR}", result.Error.Message);
            return result!.ToNotFoundProblemDetails();
        }
    }
}