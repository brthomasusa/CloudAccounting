using CloudAccounting.Application.UseCases.IdentityManagement.UpdateUserRole;

namespace CloudAccounting.Web.EndPoints.IdentityManagement.Authorization;

public class UpdateUserRole : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("identity/roles/", UpdateUserRoleHandler)
            .Produces(401)
            .Produces(403)
            .Produces(204)
            // .RequireAuthorization()
            .Produces(500);
    }

    // [Authorize(Roles = "AppAdmin")]
    public static async Task<IResult> UpdateUserRoleHandler(UpdateUserRoleCommand command, ISender sender, ILogger<UpdateUserRole> logger)
    {
        Result<Unit> result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        string msg = result.Error.Message;
        logger.LogWarning("There was a problem updating the user role: {ERROR}", msg);
        return Results.BadRequest(msg);
    }
}   
