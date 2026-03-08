using CloudAccounting.Shared.VoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.CreateVoucherType;

namespace CloudAccounting.Web.EndPoints.VoucherTypes
{
    public class CreateVoucherType : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("vouchertypes/", CreateVoucherTypeFromCommand)
                .Produces(400)
                .Produces<VoucherTypeDto>(200)
                .Produces(500);
        }

        public static async Task<IResult> CreateVoucherTypeFromCommand(
            CreateVoucherTypeCommand command,
            ISender sender,
            ILogger<CreateVoucherType> logger
        )
        {
            Result<VoucherTypeDto>? result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem creating the voucher type: {ERROR}", msg);
            return Results.BadRequest(msg);
        }
    }
}