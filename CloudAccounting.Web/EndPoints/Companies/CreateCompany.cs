using CloudAccounting.Application.UseCases.Companies.CreateCompany;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.Web.EndPoints.Companies;

public class CreateCompany : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("companies/", CreateCompanyFromCommand)
            .Produces(400)
            .Produces<CompanyDetailDto>(201)
            .Produces(500);
    }

    public static async Task<IResult> CreateCompanyFromCommand([FromBody] CreateCompanyCommand command, ISender sender, ILogger<CreateCompany> logger)
    {

        Result<CompanyDetailDto>? result = await sender.Send(command);

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
        return Results.BadRequest(msg);
    }
}
