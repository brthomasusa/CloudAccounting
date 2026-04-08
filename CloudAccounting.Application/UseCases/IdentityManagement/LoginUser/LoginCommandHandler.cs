using CloudAccounting.Infrastructure.Data.Services;
using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.LoginUser
{
    public class LoginCommandHandler
    (
        AuthService authService,
        ILogger<LoginCommandHandler> logger
    ) : ICommandHandler<LoginCommand, LoginResponseModel>
    {
        private readonly AuthService _authService = authService;
        private readonly ILogger<LoginCommandHandler> _logger = logger;

        public async Task<Result<LoginResponseModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Result<LoginResponseModel> result = await _authService.LoginAsync(request.Email, request.Password);

            if (result.IsFailure)
            {
                string errMsg = result.Error.Message;
                _logger.LogError("Error logging in user: {Message}", errMsg);
                return Result<LoginResponseModel>.Failure<LoginResponseModel>(new Error("LoginCommandHandler.Handle", errMsg));
            }

            return Result<LoginResponseModel>.Success(result.Value);
        }
    }
}