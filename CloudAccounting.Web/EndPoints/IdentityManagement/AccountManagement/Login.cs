using CloudAccounting.Application.UseCases.IdentityManagement.LoginUser;

namespace CloudAccounting.Web.EndPoints.IdentityManagement.AccountManagement
{
    public class Login : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("identity/login", LoginUserFromCommand)
                .Produces(400)
                .Produces<LoginResponse>(200)
                .Produces(500);
        }

        public static async Task<IResult> LoginUserFromCommand(
            LoginCommand command,
            ISender sender,
            ILogger<Login> logger
        )
        {
            Result<LoginResponse>? result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem logging in the user: {ERROR}", msg);
            return Results.Unauthorized();
        }
    }
}