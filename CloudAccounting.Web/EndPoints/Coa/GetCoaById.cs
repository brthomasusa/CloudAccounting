using CloudAccounting.Application.UseCases.Coa.GetByAccount;
using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Web.EndPoints.Coa;

public class GetCoaById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("coa/{companyCode:int}/{accountCode}", GetChartOfAccountByAccountCode)
            .Produces(404)
            .Produces<ChartOfAccountDto>(200)
            .Produces(500);
    }

    public static async Task<IResult> GetChartOfAccountByAccountCode(int companyCode, string accountCode,
        ISender sender,
        ILogger<GetCoaById> logger)
    {
        GetChartOfAccountByAccountCodeQuery query = new(companyCode, accountCode);
        var result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem getting the chart of account: {ERROR}", msg);
        return Results.NotFound(msg);
    }
}