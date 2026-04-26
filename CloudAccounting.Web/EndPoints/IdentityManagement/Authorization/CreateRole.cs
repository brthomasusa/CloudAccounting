using CloudAccounting.Application.UseCases.IdentityManagement.CreateRole;

namespace CloudAccounting.Web.EndPoints.IdentityManagement.Authorization
{
    public class CreateRole : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("identity/roles/", CreateRoleHandler)
                .Produces(401)
                .Produces(403)
                .Produces(204)
                // .RequireAuthorization()
                .Produces(500);
        }

        // [Authorize(Roles = "AppAdmin")]
        public static async Task<IResult> CreateRoleHandler(CreateRoleCommand command, ISender sender, ILogger<CreateRole> logger)
        {
            Result<MediatR.Unit>? result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.NoContent();
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem creating the role: {ERROR}", msg);
            return Results.BadRequest(msg);
        }
    }
}