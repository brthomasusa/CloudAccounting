using CloudAccounting.Application.UseCases.Companies.DeleteCompany;

namespace CloudAccounting.Web.EndPoints.Companies;

public class DeleteCompany : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("companies", DeleteCompanyFromCommand)
            .Produces(404)
            .Produces(204)
            .Produces(500);
    }

    public static async Task<IResult> DeleteCompanyFromCommand
    (
        [FromBody] DeleteCompanyCommand command,
        ISender sender,
        ILogger<DeleteCompany> logger
    )
    {
        Result<MediatR.Unit> result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        string msg = result.Error.Message;
        logger.LogWarning("There was a problem deleting the company with code {CODE}: {ERROR}", command.CompanyCode, msg);
        return Results.NotFound(msg);
    }
}

