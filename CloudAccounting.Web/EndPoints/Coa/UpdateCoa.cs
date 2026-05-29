using CloudAccounting.Application.UseCases.Coa.Update;
using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Web.EndPoints.Coa
{
    public class UpdateCoa : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("coa/", UpdateCoaFromCommand)
                .Produces(400)
                .Produces<ChartOfAccountDto>()
                .Produces(500);
        }

        public static async Task<IResult> UpdateCoaFromCommand(
            UpdateChartOfAccountCommand command,
            ISender sender,
            ILogger<UpdateCoa> logger
        )
        {
            var result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            var msg = result.Error.Message;
            logger.LogWarning("There was a problem updating the account with code {ACCOUNT_CODE}: {ERROR}",
                command.AccountCode, msg);
            return Results.BadRequest(msg);
        }
    }
}