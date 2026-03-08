using CloudAccounting.Application.UseCases.Companies.GetAllCompanies;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.Web.EndPoints.Companies;

public class GetAllCompanies : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("companies/", GetAllCompaniesFromQuery)
            .Produces(404)
            .Produces<List<CompanyDetailDto>>(200)
            .Produces(500);
    }

    public static async Task<IResult> GetAllCompaniesFromQuery(ISender sender, ILogger<GetAllCompanies> logger)
    {
        GetAllCompaniesQuery query = new(1, 10);
        Result<List<CompanyDetailDto>>? result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        string msg = result.Error.Message;
        logger.LogWarning("There was a problem getting all companies: {ERROR}", msg);
        return Results.NotFound(msg);
    }
}
