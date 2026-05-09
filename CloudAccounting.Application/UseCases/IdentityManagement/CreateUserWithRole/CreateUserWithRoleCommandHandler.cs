using CloudAccounting.Infrastructure.Data.Services;

namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateUserWithRole
{
    public class CreateUserWithRoleCommandHandler
    (
        AuthenticationService authenticationService,
        ILogger<CreateUserWithRoleCommandHandler> logger
    ) : ICommandHandler<CreateUserWithRoleCommand, MediatR.Unit>
    {
        public async Task<Result<MediatR.Unit>> Handle(CreateUserWithRoleCommand request, CancellationToken cancellationToken)
        {
            Result<User> result = await authenticationService.CreateUserWithRoleAsync(
                request.Email,
                request.Password,
                request.CompanyCode,
                request.RoleName
            );

            if (result.IsFailure)
            {
                string errMsg = result.Error.Message;
                logger.LogError("Error creating user with role: {Message}", errMsg);

                return Result.Failure<MediatR.Unit>(new Error("CreateUserWithRoleCommandHandler.Handle", errMsg));
            }

            return Result.Success(MediatR.Unit.Value);
        }
    }
}