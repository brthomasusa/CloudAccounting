using CloudAccounting.Application.UseCases.Company.GetNextValidFiscalYearStart;

namespace CloudAccounting.Web.EndPoints.Company
{
    public class GetNextValidFiscalYearStartDate : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("companies/validstartdate/{companyCode:int}", GetNextValidFiscalYearStart)
                .Produces<DateTime>()
                .Produces(500);
        }

        public static async Task<IResult> GetNextValidFiscalYearStart(int companyCode, ISender sender, ILogger<CreateCompany> logger)
        {
            GetNextValidFiscalYearStartDateQuery query = new(companyCode);
            Result<DateTime>? result = null;

            try
            {
                result = await sender.Send(query);

                if (result.IsSuccess)
                {
                    return TypedResults.Ok(result.Value);
                }

                string msg = result!.Error.Message;
                logger.LogWarning("There was a problem retrieving next valid fiscal year start date: {ERROR}", msg);
                return result!.ToInternalServerErrorProblemDetails(msg);

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