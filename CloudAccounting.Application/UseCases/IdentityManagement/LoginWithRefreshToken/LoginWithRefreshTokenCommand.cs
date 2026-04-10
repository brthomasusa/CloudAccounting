using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.LoginWithRefreshToken
{
    public record class LoginWithRefreshTokenCommand(string RefreshToken) : ICommand<LoginResponseModel>;
}