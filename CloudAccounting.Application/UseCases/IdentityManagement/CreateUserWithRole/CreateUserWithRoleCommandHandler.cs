using CloudAccounting.Infrastructure.Data.Services;

namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateUserWithRole
{
    public class CreateUserWithRoleCommandHandler
    (
        AuthenticationService authenticationService,
        ILogger<CreateUserWithRoleCommandHandler> logger
    ) : ICommandHandler<CreateUserWithRoleCommand, MediatR.Unit>
    {
        private readonly AuthenticationService _authenticationService = authenticationService;
        private readonly ILogger<CreateUserWithRoleCommandHandler> _logger = logger;

        public async Task<Result<MediatR.Unit>> Handle(CreateUserWithRoleCommand request, CancellationToken cancellationToken)
        {
            Result<User> result = await _authenticationService.CreateUserWithRoleAsync(
                request.Email,
                request.Password,
                request.CompanyCode,
                request.RoleName,
                request.IsSystemAdmin,
                request.IsCompanyAdmin
            );

            if (result.IsFailure)
            {
                string errMsg = result.Error.Message;
                _logger.LogError("Error creating user with role: {Message}", errMsg);

                return Result<MediatR.Unit>.Failure<MediatR.Unit>(new Error("CreateUserWithRoleCommandHandler.Handle", errMsg));
            }

            return Result<MediatR.Unit>.Success(MediatR.Unit.Value);
        }
    }
}