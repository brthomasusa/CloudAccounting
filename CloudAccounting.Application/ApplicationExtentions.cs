using CloudAccounting.Application.Behaviors;
using CloudAccounting.Application.UseCases.Companies.CreateCompany;
using CloudAccounting.Application.UseCases.Companies.DeleteCompany;
using CloudAccounting.Application.UseCases.Companies.UpdateCompany;
using CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear;
using CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;
using CloudAccounting.Application.UseCases.VoucherTypes.CreateVoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.DeleteVoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.UpdateVoucherType;
using CloudAccounting.Application.UseCases.IdentityManagement.RegisterUser;
using CloudAccounting.Application.UseCases.IdentityManagement.LoginUser;
using Microsoft.Extensions.DependencyInjection;

namespace CloudAccounting.Application
{
    public static class ApplicationExtentions
    {
        public static void RegisterValidators(this IServiceCollection services)
        {
            services
                .AddScoped<IValidator<CreateCompanyCommand>, CreateCompanyValidator>()
                .AddScoped<IValidator<UpdateCompanyCommand>, UpdateCompanyValidator>()
                .AddScoped<IValidator<DeleteCompanyCommand>, DeleteCompanyValidator>()
                .AddScoped<IValidator<CreateFiscalYearCommand>, CreateFiscalYearCommandValidator>()
                .AddScoped<IValidator<DeleteFiscalYearCommand>, DeleteFiscalYearCommandValidator>()
                .AddScoped<IValidator<CreateVoucherTypeCommand>, CreateVoucherTypeCommandValidator>()
                .AddScoped<IValidator<UpdateVoucherTypeCommand>, UpdateVoucherTypeCommandValidator>()
                .AddScoped<IValidator<DeleteVoucherTypeCommand>, DeleteVoucherTypeCommandValidator>()
                .AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>()
                .AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();

        }

        public static void AddMediatr(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly);
                config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            });
        }
    }
}