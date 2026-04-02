using CloudAccounting.Application.UseCases.IdentityManagement.RegisterUser;

namespace CloudAccounting.Web.EndPoints.IdentityManagement.AccountManagement
{
    public class Register : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("identity/register", RegisterUserFromCommand)
                .Produces(400)
                .Produces<RegisterUserResponse>(200)
                .Produces(500);
        }

        public static async Task<IResult> RegisterUserFromCommand(
            RegisterUserCommand command,
            ISender sender,
            ILogger<Register> logger
        )
        {
            Result<RegisterUserResponse>? result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            string msg = result.Error.Message;
            logger.LogWarning("There was a problem registering the user: {ERROR}", msg);
            return Results.BadRequest(msg);
        }
    }
}