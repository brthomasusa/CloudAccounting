using CloudAccounting.Application.UseCases.Company.DeleteFiscalYear;

namespace CloudAccounting.Web.EndPoints.Company
{
    public class DeleteFiscalYear : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("companies/fiscalyear", DeleteCompanyFiscalYear);
        }

        public static async Task<IResult> DeleteCompanyFiscalYear([FromBody] DeleteFiscalYearCommand command, ISender sender, ILogger<DeleteCompany> logger)
        {
            Result<MediatR.Unit>? result = null;

            try
            {
                result = await sender.Send(command);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                string msg = result.Error.Message;
                logger.LogWarning("There was a problem deleting fiscal year data for '{COMPANYCODE} (fiscal year: {FISCALYEAR})': {ERROR}", command.CompanyCode, command.FiscalYear, result.Error.Message);
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
}