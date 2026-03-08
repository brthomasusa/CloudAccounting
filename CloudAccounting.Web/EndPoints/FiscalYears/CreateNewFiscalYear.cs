using CloudAccounting.Shared.FiscalYear;
using CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear;

namespace CloudAccounting.Web.EndPoints.FiscalYears
{
    public class CreateNewFiscalYear : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("fiscalyears/", CreateFiscalYear)
                .Produces(400)
                .Produces<FiscalYearDto>()
                .Produces(500);
        }

        public static async Task<IResult> CreateFiscalYear([FromBody] CreateFiscalYearCommand command, ISender sender, ILogger<CreateNewFiscalYear> logger)
        {
            Result<FiscalYearDto>? result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return TypedResults.Ok(result.Value);
            }

            logger.LogWarning("There was a problem creating the new fiscal year: {ERROR}", result.Error.Message);
            return result!.ToBadRequestProblemDetails();
        }
    }
}