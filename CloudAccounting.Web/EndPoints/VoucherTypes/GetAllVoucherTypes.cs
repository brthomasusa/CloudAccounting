using CloudAccounting.Shared.VoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypes;

namespace CloudAccounting.Web.EndPoints.VoucherTypes
{
    public class GetAllVoucherTypes : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("vouchertypes/", GetAllVoucherTypesFromQuery)
                .Produces(404)
                .Produces<List<VoucherTypeDto>>(200)
                .Produces(500);
        }

        public static async Task<IResult> GetAllVoucherTypesFromQuery(ISender sender, ILogger<GetAllVoucherTypes> logger)
        {
            GetAllVoucherTypesQuery query = new();
            Result<List<VoucherTypeDto>>? result = await sender.Send(query);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem getting all voucher types: {ERROR}", msg);
            return Results.NotFound(msg);
        }

    }
}