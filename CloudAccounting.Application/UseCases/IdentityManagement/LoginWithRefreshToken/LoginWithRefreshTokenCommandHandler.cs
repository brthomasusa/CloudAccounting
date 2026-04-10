using CloudAccounting.Infrastructure.Data.Services;
using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.LoginWithRefreshToken
{
    public class LoginWithRefreshTokenCommandHandler
    (
        AuthService authService,
        ILogger<LoginWithRefreshTokenCommandHandler> logger
    ) : ICommandHandler<LoginWithRefreshTokenCommand, LoginResponseModel>
    {
        private readonly AuthService _authService = authService;
        private readonly ILogger<LoginWithRefreshTokenCommandHandler> _logger = logger;

        public async Task<Result<LoginResponseModel>> Handle(LoginWithRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            Result<LoginResponseModel> result = await _authService.LoginWithRefreshTokenAsync(request.RefreshToken);

            if (result.IsFailure)
            {
                string errMsg = result.Error.Message;
                _logger.LogError("Error logging in user: {Message}", errMsg);
                return Result<LoginResponseModel>.Failure<LoginResponseModel>(new Error("LoginCommandHandler.Handle", errMsg));
            }

            return result.Value;
        }
    }
}