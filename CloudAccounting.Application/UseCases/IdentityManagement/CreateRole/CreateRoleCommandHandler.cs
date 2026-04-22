using Microsoft.AspNetCore.Identity;
using CloudAccounting.Infrastructure.Data.Services;

namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateRole
{
    public class CreateRoleCommandHandler
    (
        AuthorizationService authorizationService,
        ILogger<CreateRoleCommandHandler> logger
    ) : ICommandHandler<CreateRoleCommand, MediatR.Unit>
    {
        public async Task<Result<MediatR.Unit>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Result<MediatR.Unit> result = await authorizationService.CreateRoleAsync(request.RoleName);

                if (result.IsSuccess)
                {
                    return Result.Success(MediatR.Unit.Value);
                }

                logger.LogError("Error creating role: {Message}", result.Error.Message);

                return Result.Failure<MediatR.Unit>(new Error("CreateRoleCommandHandler.Handle", result.Error.Message));
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<MediatR.Unit>(new Error("CreateRoleCommandHandler.Handle", errMsg));
            }
        }
    }
}