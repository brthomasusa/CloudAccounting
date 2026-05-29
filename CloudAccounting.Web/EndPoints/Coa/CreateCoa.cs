using CloudAccounting.Application.UseCases.Coa.Create;

namespace CloudAccounting.Web.EndPoints.Coa;

public class CreateCoa : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("coa", CreateCoaFromBody)
            .Produces(201)
            .Produces(400)
            .Produces(500);
    }

    public static async Task<IResult> CreateCoaFromBody
    (
        CreateChartOfAccountCommand command,
        ISender sender,
        ILogger<CreateCoa> logger
    )
    {
        var result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.Created($"coa/{result.Value.CompanyCode}/{result.Value.AccountCode}", result.Value);
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem creating the account: {ERROR}", msg);
        return Results.BadRequest(msg);
    }
}
