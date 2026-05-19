using CloudAccounting.Application.UseCases.CostCenters.GetCostCenter;
using CloudAccounting.Shared;

namespace CloudAccounting.Web.EndPoints.CostCenters;

public class GetCostCenterByCode : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("costcenters/{companyCode:int}/{costCenterCode}", GetCostCenterByCodeFromQuery)
            .Produces(404)
            .Produces<CostCenterDto>(200)
            .Produces(500);
    }

    // [Authorize(Roles = "CompanyAdmin")]
    public static async Task<IResult> GetCostCenterByCodeFromQuery(int companyCode, string costCenterCode,
        ISender sender,
        ILogger<GetCostCenterByCode> logger)
    {
        GetCostCenterQuery query = new(companyCode, costCenterCode);
        var result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem getting the cost center: {ERROR}", msg);
        return Results.NotFound(msg);
    }
}