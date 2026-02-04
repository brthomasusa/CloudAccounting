using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Application.UseCases.CreateCompany;

namespace CloudAccounting.Web.EndPoints.Company
{
    public class CreateCompany : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("companies/", CreateCompanyFromCommand);
        }

        public static async Task<IResult> CreateCompanyFromCommand([FromBody] CreateCompanyCommand command, ISender sender, ILogger<CreateCompany> logger)
        {
            Result<CompanyDetailVm>? result = null;

            try
            {
                result = await sender.Send(command);

                if (result.IsSuccess)
                {
                    return Results.CreatedAtRoute(
                        routeName: "GetCompanyByCompanyCode",
                        routeValues: new { companyCode = result.Value.CompanyCode },
                        result.Value
                    );
                }

                string msg = result.Error.Message;
                logger.LogWarning("There was a problem creating the new company: {ERROR}", result.Error.Message);
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