using CloudAccounting.Shared.VoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.DeleteVoucherType;

namespace CloudAccounting.Web.EndPoints.VoucherTypes
{
    public class DeleteVoucherType : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("vouchertypes/", DeleteVoucherTypeFromCommand)
                .Produces(404)
                .Produces(200)
                .Produces(500);
        }

        public static async Task<IResult> DeleteVoucherTypeFromCommand(
            [FromBody] DeleteVoucherTypeCommand command,
            ISender sender,
            ILogger<DeleteVoucherType> logger
        )
        {
            Result<MediatR.Unit> result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem deleting the voucher type with code {VOUCHER_CODE}: {ERROR}", command.VoucherCode, msg);
            return Results.NotFound(msg);
        }
    }
}