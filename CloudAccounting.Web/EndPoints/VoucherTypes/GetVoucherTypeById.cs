using CloudAccounting.Shared.VoucherType;
using CloudAccounting.Application.UseCases.VoucherType.GetVoucherTypeById;

namespace CloudAccounting.Web.EndPoints.VoucherTypes
{
    public class GetVoucherTypeById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("vouchertypes/{voucherCode:int}", GetVoucherType)
                .Produces(404)
                .Produces<List<VoucherTypeDto>>()
                .Produces(500)
                .WithName("GetVoucherType");
        }

        public static async Task<IResult> GetVoucherType(int voucherCode, ISender sender, ILogger<GetVoucherTypeById> logger)
        {
            Result<VoucherTypeDto>? result = null;

            try
            {
                result = await sender.Send(new GetVoucherTypeByIdQuery(voucherCode));

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                string msg = result.Error.Message;
                logger.LogWarning("There was a problem retrieving the voucher type with voucher code {CODE}: {ERROR}", voucherCode, result.Error.Message);
                return Results.NotFound(msg);

            }
            catch (Exception ex)
            {
                string msg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", msg);
                return result!.ToInternalServerErrorProblemDetails(msg);
            }
        }
    }
}