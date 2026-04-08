using CloudAccounting.Application;
using CloudAccounting.Infrastructure.Data.Repositories;
using CloudAccounting.Infrastructure.Data.Services;
using CloudAccounting.Web.EndPoints;
using Mapster;
using MapsterMapper;
using Microsoft.OpenApi;

namespace CloudAccounting.Web.Extentions;

public static class DependencyInjection
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CloudAccounting.Web.Policy", builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
            );
        });

    public static void AddSwaggerGenAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });
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
        RouteGroupBuilder basicGroup = app.MapGroup("/api/v1").WithTags("Cloud Accounting API v1");

        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? basicGroup : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    public static void AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(ApplicationAssembly.Instance, InfrastructureAssembly.Instance);
        config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    public static void AddRepositoriesAndDomainServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<ICompanyRepository, CompanyRepository>()
            .AddScoped<IFiscalYearRepository, FiscalYearRepository>()
            .AddScoped<IVoucherTypeRepository, VoucherTypeRepository>()
            .AddScoped<ILookupRepository, LookupRepository>()
            .AddScoped<IFiscalYearService, FiscalYearService>()
            .AddScoped<IIdentityMgmtRepository, IdentityMgmtRepository>()
            .AddScoped<AuthService>();
    }
}
