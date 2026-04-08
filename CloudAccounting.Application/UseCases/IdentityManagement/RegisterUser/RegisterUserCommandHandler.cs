using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.RegisterUser
{
    public class RegisterUserCommandHandler
    (
        IIdentityMgmtRepository identityMgmtRepository,
        ILogger<RegisterUserCommandHandler> logger
    ) : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IIdentityMgmtRepository _identityMgmtRepository = identityMgmtRepository;
        private readonly ILogger<RegisterUserCommandHandler> _logger = logger;

        public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            Result<bool> result =
                await _identityMgmtRepository.RegisterUserAsync(
                    request.Email,
                    request.Password,
                    request.PhoneNumber,
                    request.CompanyCode,
                    request.IsAdministrator
                );

            if (result.IsFailure)
            {
                string errMsg = result.Error.Message;
                _logger.LogError("Error registering user: {Message}", errMsg);
                return Result<RegisterUserResponse>.Failure<RegisterUserResponse>(new Error("RegisterUserCommandHandler.Handle", errMsg));
            }

            return Result<RegisterUserResponse>.Success(new RegisterUserResponse(request.Email, request.Email));
        }
    }
}