using CloudAccounting.Application;
using CloudAccounting.Application.Behaviors;
using CloudAccounting.Application.UseCases.Companies.CreateCompany;
using CloudAccounting.Application.UseCases.Companies.DeleteCompany;
using CloudAccounting.Application.UseCases.Companies.UpdateCompany;
using CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear;
using CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;
using CloudAccounting.Application.UseCases.VoucherTypes.CreateVoucherType;
using CloudAccounting.Infrastructure.Data.Repositories;
using CloudAccounting.Infrastructure.Data.Services;
using CloudAccounting.Web.EndPoints;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace CloudAccounting.Web.Extentions;

public static class DependencyInjection
{
    public static void ConfigureAuthentication(this IServiceCollection services) =>
        services
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // options.Authority = "https://your-identity-server.com";
                // options.Audience = "your-api-resource";
            });

    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CloudAccounting.Web.Policy", builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
            );
        });

    public static void AddCustomSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen(c =>
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Description = "Cloud Accounting",
                Title = "CloudAccounting API",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "CloudAccounting"
                }
            }
        ));

    public static void UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CloudAccounting v1"));
    }

    public static void AddInfrastructureDataLayer(this WebApplicationBuilder builder)
    {
        string? connectionString =
             builder.Configuration["ConnectionStrings:CloudAcctgTest"] ??
                throw new ArgumentNullException("Db connection string is null.");

        builder.Services
            .AddDbContext<CloudAccountingContext>(options =>
                    options.UseSqlServer(connectionString)
                           .EnableSensitiveDataLogging()
                           .EnableDetailedErrors()
                );
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = [.. assembly
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                            type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))];

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    public static void AddRepositoriesAndDomainServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<ICompanyRepository, CompanyRepository>()
            .AddScoped<IFiscalYearRepository, FiscalYearRepository>()
            .AddScoped<IVoucherTypeRepository, VoucherTypeRepository>()
            .AddScoped<ILookupRepository, LookupRepository>()
            .AddScoped<IFiscalYearService, FiscalYearService>();
    }

    public static void AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(ApplicationAssembly.Instance, InfrastructureAssembly.Instance);
        config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }
}
