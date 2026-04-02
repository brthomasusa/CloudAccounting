using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAccounting.Application.UseCases.IdentityManagement.RegisterUser
{
    public record class RegisterUserResponse
    (
        string UserId,
        string Email
    );
}