using CloudAccounting.Infrastructure.Data.IdentityModels;

namespace CloudAccounting.Application.UseCases.IdentityManagement.LoginUser
{
    public class LoginCommandHandler
    (
        IIdentityMgmtRepository identityMgmtRepository,
        ILogger<LoginCommandHandler> logger
    ) : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityMgmtRepository _identityMgmtRepository = identityMgmtRepository;
        private readonly ILogger<LoginCommandHandler> _logger = logger;

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Login login = request.Adapt<Login>();

            Result<string> result = await _identityMgmtRepository.LoginUserAsync(login);

            if (result.IsFailure)
            {
                string errMsg = result.Error.Message;
                _logger.LogError("Error logging in user: {Message}", errMsg);
                return Result<LoginResponse>.Failure<LoginResponse>(new Error("LoginCommandHandler.Handle", errMsg));
            }

            return new LoginResponse(result.Value);
        }
    }
}