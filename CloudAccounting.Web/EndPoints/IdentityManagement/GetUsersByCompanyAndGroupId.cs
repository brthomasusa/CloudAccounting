using CloudAccounting.Application.UseCases.IdentityManagement.GetUsersByCompanyAndGroup;
using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Web.EndPoints.IdentityManagement;

public class GetUsersByCompanyAndGroupId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("identity/users/{companyCode:int}/{groupId:int}", GetUsersByCompanyAndGroup)
            .Produces(401)
            .Produces(403)
            .Produces(404)
            .Produces(200)
            .Produces(500);
    }

    public static async Task<IResult> GetUsersByCompanyAndGroup
        (
            int companyCode, 
            int groupId, ISender sender, ILogger<GetUsersByCompanyAndGroupId> logger
        )
    {
        Result<List<UserModel>> result = await sender.Send(new GetUsersByCompanyAndGroupIdQuery(companyCode, groupId));

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem retrieving the users: {ERROR}", msg);
        return Results.BadRequest(msg);
    }   
}
