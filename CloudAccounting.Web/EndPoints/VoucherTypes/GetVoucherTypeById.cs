using CloudAccounting.Shared.VoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypeById;

namespace CloudAccounting.Web.EndPoints.VoucherTypes
{
    public class GetVoucherTypeById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("vouchertypes/{voucherCode:int}", GetVoucherTypeByIdFromQuery)
                .Produces(404)
                .Produces<VoucherTypeDto>(200)
                .Produces(500);
        }

        public static async Task<IResult> GetVoucherTypeByIdFromQuery(
            int voucherCode,
            ISender sender,
            ILogger<GetVoucherTypeById> logger
        )
        {
            GetVoucherTypeByIdQuery query = new(voucherCode);
            Result<VoucherTypeDto>? result = await sender.Send(query);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem getting the voucher type with code {VOUCHER_CODE}: {ERROR}", voucherCode, msg);
            return Results.NotFound(msg);
        }
    }
}