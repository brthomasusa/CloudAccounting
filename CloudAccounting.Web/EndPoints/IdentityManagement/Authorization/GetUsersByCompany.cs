
using CloudAccounting.Application.UseCases.IdentityManagement.GetAllUsers;
using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Web.EndPoints.IdentityManagement.Authorization;

public class GetUsersByCompany : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("identity/users/{companyCode:int}", GetUsersByCompanyHandler)
            .Produces(401)
            .Produces(403)
            .Produces(404)
            .Produces(200)
            .Produces(500);
    }

    public static async Task<IResult> GetUsersByCompanyHandler(int companyCode, ISender sender, ILogger<GetUsersByCompany> logger)
    {
        Result<List<UserModel>> result = await sender.Send(new GetAllUsersQuery(companyCode));

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem retrieving the users: {ERROR}", msg);
        return Results.BadRequest(msg);
    }
}
