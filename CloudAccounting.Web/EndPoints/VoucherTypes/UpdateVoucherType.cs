using CloudAccounting.Shared.VoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.UpdateVoucherType;

namespace CloudAccounting.Web.EndPoints.VoucherTypes
{
    public class UpdateVoucherType : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("vouchertypes/", UpdateVoucherTypeFromCommand)
                .Produces(400)
                .Produces<VoucherTypeDto>(200)
                .Produces(500);
        }

        public static async Task<IResult> UpdateVoucherTypeFromCommand(
            UpdateVoucherTypeCommand command,
            ISender sender,
            ILogger<UpdateVoucherType> logger
        )
        {
            Result<VoucherTypeDto>? result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem updating the voucher type with code {VOUCHER_CODE}: {ERROR}", command.VoucherCode, msg);
            return Results.BadRequest(msg);
        }
    }
}