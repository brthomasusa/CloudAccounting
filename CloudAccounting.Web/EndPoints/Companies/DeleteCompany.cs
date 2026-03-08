using CloudAccounting.Application.UseCases.Companies.DeleteCompany;

namespace CloudAccounting.Web.EndPoints.Companies;

public class DeleteCompany : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("companies/{companyCode:int}", DeleteCompanyFromCommand)
            .Produces(400)
            .Produces(204)
            .Produces(500);
    }

    public static async Task<IResult> DeleteCompanyFromCommand([FromRoute] int companyCode, ISender sender, ILogger<DeleteCompany> logger)
    {
        DeleteCompanyCommand command = new() { CompanyCode = companyCode };
        Result<MediatR.Unit> result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        string msg = result.Error.Message;
        logger.LogWarning("There was a problem deleting the company with code {CODE}: {ERROR}", companyCode, msg);
        return Results.BadRequest(msg);
    }
}
