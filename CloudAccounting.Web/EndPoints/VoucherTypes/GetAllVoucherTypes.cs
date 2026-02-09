using CloudAccounting.Shared.VoucherType;
using CloudAccounting.Application.UseCases.VoucherType.GetVoucherTypes;

namespace CloudAccounting.Web.EndPoints.VoucherTypes
{
    public class GetAllVoucherTypes : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("vouchertypes", GetVoucherTypes)
                .Produces<List<VoucherTypeDto>>()
                .Produces(500);
        }

        public static async Task<IResult> GetVoucherTypes
        (
            ISender sender,
            ILogger<GetAllVoucherTypes> logger
        )
        {
            Result<List<VoucherTypeDto>>? result = null;
            try
            {
                result = await sender.Send(new GetAllVoucherTypesQuery())
                    ;

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                logger.LogWarning("There was a problem retrieving the list of voucher types: {Message}", result.Error.Message);
                return result!.ToInternalServerErrorProblemDetails(result.Error.Message);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "There was a problem retrieving the list of voucher types: {Message}", errMsg);
                return result!.ToInternalServerErrorProblemDetails(errMsg);
            }
        }
    }
}