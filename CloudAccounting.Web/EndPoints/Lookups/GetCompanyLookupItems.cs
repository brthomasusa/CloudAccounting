using CloudAccounting.Shared.Lookups;
using CloudAccounting.Application.UseCases.Lookups.GetCompanyLookupItems;

namespace CloudAccounting.Web.EndPoints.Lookups
{
    public class GetCompanyLookupItems : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("lookups/companycodes/", GetCompanyAllLookupItems)
                .Produces(401)
                .Produces(403)
                .Produces(404)
                .Produces<List<CompanyLookupItem>>(200).RequireAuthorization()
                .Produces(500); // .Produces<List<CompanyLookupItem>>(200).RequireAuthorization()
        }

        // [Authorize(Roles = "AppAdmin")]
        public static async Task<IResult> GetCompanyAllLookupItems(ISender sender, ILogger<GetCompanyLookupItems> logger)
        {
            GetCompanyLookupItemsQuery query = new();
            Result<List<CompanyLookupItem>>? result = await sender.Send(query);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem getting all company lookup items: {ERROR}", msg);
            return Results.NotFound(msg);
        }

    }
}