using CloudAccounting.Shared.Lookups;
using CloudAccounting.Application.UseCases.Lookups.GetCostCenterLookupItems;

namespace CloudAccounting.Web.EndPoints.Lookups;

public class GetCostCenterLookupItems : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("lookups/costcenters/{companyCode:int}", GetCostCenterLookupItemsByCompanyCode)
            .Produces(401)
            .Produces(403)
            .Produces(404)
            .Produces<List<CostCenterLookupItem>>() //.RequireAuthorization()
            .Produces(500); // .Produces<List<CostCenterLookupItem>>(200).RequireAuthorization()
    }

    // [Authorize(Roles = "AppAdmin")]
    public static async Task<IResult> GetCostCenterLookupItemsByCompanyCode(
        int companyCode,
        ISender sender,
        ILogger<GetCostCenterLookupItems> logger
    )
    {
        GetCostCenterLookupItemQuery query = new(companyCode);
        Result<List<CostCenterLookupItem>> result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        string msg = result.Error.Message;
        logger.LogWarning(
            "There was a problem getting cost center lookup items for company code {CompanyCode}: {ERROR}", companyCode,
            msg);
        return Results.NotFound(msg);
    }
}