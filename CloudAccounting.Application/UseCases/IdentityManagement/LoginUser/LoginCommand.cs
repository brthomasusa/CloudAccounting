namespace CloudAccounting.Application.UseCases.IdentityManagement.LoginUser
{
    public record class LoginCommand
    (
        string Email,
        string Password
    ) : ICommand<LoginResponse>;
}