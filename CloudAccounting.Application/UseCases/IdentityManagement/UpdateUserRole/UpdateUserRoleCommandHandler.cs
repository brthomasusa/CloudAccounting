using CloudAccounting.Infrastructure.Data.Services;

namespace CloudAccounting.Application.UseCases.IdentityManagement.UpdateUserRole;

public class UpdateUserRoleCommandHandler
    (
        AuthorizationService authorizationService,
        ILogger<UpdateUserRoleCommandHandler> logger
    ) : ICommandHandler<UpdateUserRoleCommand, MediatR.Unit>
{
        public async Task<Result<MediatR.Unit>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            Result<MediatR.Unit> result = await authorizationService.ChangeUserRoleAssignmentAsync(
                request.Email,
                request.RoleName
            );

            if (result.IsFailure)
            {
                string errMsg = result.Error.Message;
                logger.LogError("Error updating user role: {Message}", errMsg);

                return Result.Failure<MediatR.Unit>(new Error("UpdateUserRoleCommandHandler.Handle", errMsg));
            }

            return Result.Success(MediatR.Unit.Value);
        }
}