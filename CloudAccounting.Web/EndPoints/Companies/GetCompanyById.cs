using CloudAccounting.Application.UseCases.Companies.GetCompany;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.Web.EndPoints.Companies;

public class GetCompanyById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("companies/{companyCode:int}", GetCompanyByIdFromQuery)
            .WithName("GetCompanyByCompanyCode")
            .Produces(404)
            .Produces<CompanyDetailDto>(200)
            .Produces(500);
    }

    public static async Task<IResult> GetCompanyByIdFromQuery([FromRoute] int companyCode, ISender sender, ILogger<GetCompanyById> logger)
    {
        GetCompanyByIdQuery query = new(companyCode);
        Result<CompanyDetailDto>? result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        string msg = result.Error.Message;
        logger.LogWarning("There was a problem getting the company with code {CODE}: {ERROR}", companyCode, result.Error.Message);
        return Results.NotFound(msg);
    }
}
