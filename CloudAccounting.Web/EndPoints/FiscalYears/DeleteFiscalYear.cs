using CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;

namespace CloudAccounting.Web.EndPoints.FiscalYears
{
    public class DeleteFiscalYear : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("fiscalyears/", DeleteFiscalYearAsync)
                .Produces(404)
                .Produces(204)
                .Produces(500);
        }

        public static async Task<IResult> DeleteFiscalYearAsync
        (
            [FromBody] DeleteFiscalYearCommand command,
            ISender sender,
            ILogger<DeleteFiscalYear> logger
        )
        {
            Result<MediatR.Unit> result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.NoContent();
            }

            logger.LogWarning("There was a problem deleting the fiscal year: {ERROR}", result.Error.Message);
            return result!.ToNotFoundProblemDetails();
        }
    }
}
