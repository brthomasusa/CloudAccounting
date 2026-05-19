using CloudAccounting.Application.UseCases.CostCenters.GetAllCostCenters;
using CloudAccounting.Shared;

namespace CloudAccounting.Web.EndPoints.CostCenters;

public class GetAllCostCenters : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("costcenters/{companyCode:int}", GetAllCostCentersFromQuery)
            .Produces(404)
            .Produces<List<CostCenterDto>>(200)
            .Produces(500);
    }

    // [Authorize(Roles = "CompanyAdmin")]
    public static async Task<IResult> GetAllCostCentersFromQuery(int companyCode, ISender sender,
        ILogger<GetAllCostCenters> logger)
    {
        GetAllCostCentersQuery query = new(companyCode);
        var result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem getting all cost centers: {ERROR}", msg);
        return Results.NotFound(msg);
    }
}