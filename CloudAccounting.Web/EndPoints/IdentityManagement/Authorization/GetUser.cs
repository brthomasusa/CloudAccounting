using CloudAccounting.Application.UseCases.IdentityManagement.GetUserById;
using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Web.EndPoints.IdentityManagement.Authorization
{
    public class GetUser : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("identity/users/{userId}", GetUserByIdHandler)
                .Produces(401)
                .Produces(403)
                .Produces(404)
                .Produces(200)
                .Produces(500);
        }

        public static async Task<IResult> GetUserByIdHandler(string userId, ISender sender, ILogger<GetUser> logger)
        {
            Result<UserModel>? result = await sender.Send(new GetUserByIdQuery(userId));

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem retrieving the user: {ERROR}", msg);
            return Results.BadRequest(msg);
        }
    }
}