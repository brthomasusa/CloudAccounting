
using CloudAccounting.Application.UseCases.IdentityManagement.CreateUserWithRole;

namespace CloudAccounting.Web.EndPoints.IdentityManagement.Authorization
{
    public class CreateUser : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("identity/users/", CreateUserHandler)
                .Produces(401)
                .Produces(403)
                .Produces(204)
                // .RequireAuthorization()
                .Produces(500);
        }

        public static async Task<IResult> CreateUserHandler
        (
            CreateUserWithRoleCommand command,
            ISender sender,
            ILogger<CreateUser> logger
        )
        {
            Result<Unit> result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.NoContent();
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem creating the user: {ERROR}", msg);
            return Results.BadRequest(msg);
        }
    }
}