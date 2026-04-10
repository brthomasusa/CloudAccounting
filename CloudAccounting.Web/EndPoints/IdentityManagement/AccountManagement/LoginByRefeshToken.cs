using CloudAccounting.Application.UseCases.IdentityManagement.LoginWithRefreshToken;
using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Web.EndPoints.IdentityManagement.AccountManagement
{
    public class LoginByRefeshToken : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("identity/loginbyrefreshtoken", LoginWithRefreshToken)
                .Produces(400)
                .Produces<LoginResponseModel>(200).RequireAuthorization()
                .Produces(500);
        }

        public static async Task<IResult> LoginWithRefreshToken(
            [FromBody] LoginWithRefreshTokenCommand command,
            ISender sender,
            ILogger<LoginWithRefreshTokenCommandHandler> logger
        )
        {
            Result<LoginResponseModel>? result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem logging in the refresh token: {ERROR}", msg);
            return Results.Unauthorized();
        }
    }
}