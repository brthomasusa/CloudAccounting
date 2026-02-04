using CloudAccounting.Application.UseCases.Company.CreateFiscalYear;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.Web.EndPoints.Company
{
    public class CreateFiscalYear : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("companies/fiscalyear", CreateNewFiscalYear)
                .Produces(400)
                .Produces<CompanyWithFiscalPeriodsDto>()
                .Produces(500);
        }

        public static async Task<IResult> CreateNewFiscalYear([FromBody] CreateFiscalYearCommand command, ISender sender, ILogger<CreateCompany> logger)
        {
            Result<CompanyWithFiscalPeriodsDto>? result = null;

            try
            {
                result = await sender.Send(command);

                if (result.IsSuccess)
                {
                    return TypedResults.Ok(result.Value);
                }

                logger.LogWarning("There was a problem creating the new fiscal year: {ERROR}", result.Error.Message);
                return result!.ToBadRequestProblemDetails();

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