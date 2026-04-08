

namespace CloudAccounting.Application.UseCases.IdentityManagement.RegisterUser
{
    public record class RegisterUserCommand
    (
        string Email,
        string Password,
        string PhoneNumber,
        int CompanyCode,
        bool IsAdministrator
    ) : ICommand<RegisterUserResponse>;
}