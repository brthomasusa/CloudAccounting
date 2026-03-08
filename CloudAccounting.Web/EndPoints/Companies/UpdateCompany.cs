using CloudAccounting.Application.UseCases.Companies.UpdateCompany;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.Web.EndPoints.Companies;

public class UpdateCompany : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("companies/", UpdateCompanyFromCommand)
            .Produces(400)
            .Produces<CompanyDetailDto>(200)
            .Produces(500);
    }

    public static async Task<IResult> UpdateCompanyFromCommand([FromBody] UpdateCompanyCommand command, ISender sender, ILogger<UpdateCompany> logger)
    {
        Result<CompanyDetailDto>? result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        string msg = result.Error.Message;
        logger.LogWarning("There was a problem updating the company with code {CODE}: {ERROR}", command.CompanyCode, msg);
        return Results.BadRequest(msg);
    }
}
