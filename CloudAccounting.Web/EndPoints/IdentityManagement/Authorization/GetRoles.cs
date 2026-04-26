using CloudAccounting.Application.UseCases.IdentityManagement.GetAllRoles;
using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Web.EndPoints.IdentityManagement.Authorization
{
    public class GetRoles : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("identity/roles/", GetAllRolesHandler)
                .Produces(401)
                .Produces(403)
                .Produces(200)
                // .RequireAuthorization()
                .Produces(500);

        }

        public static async Task<IResult> GetAllRolesHandler(ISender sender, ILogger<GetRoles> logger)
        {
            Result<List<RoleModel>>? result = await sender.Send(new GetAllRolesQuery());

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem retrieving the roles: {ERROR}", msg);
            return Results.BadRequest(msg);
        }
    }
}