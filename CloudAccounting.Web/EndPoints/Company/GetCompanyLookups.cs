using CloudAccounting.Application.UseCases.Lookups.CompanyCodeLookup;

namespace CloudAccounting.Web.EndPoints.Company
{
    public class GetCompanyLookups : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("companies/lookups", GetCompanyLookup);
        }

        public static async Task<IResult> GetCompanyLookup(ISender sender, ILogger<GetCompanyById> logger)
        {
            Result<List<CompanyLookup>>? result = null;

            try
            {
                result = await sender.Send(new CompanyCodeLookupQuery());

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                string msg = result.Error.Message;
                logger.LogWarning("There was a problem retrieving the company lookup codes: {ERROR}", result.Error.Message);
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