using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi;
using CloudAccounting.Application;
using CloudAccounting.Application.Behaviors;
using CloudAccounting.Application.Validators.Company;
using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Core.Repositories;
using CloudAccounting.Infrastructure.Data;
using CloudAccounting.Infrastructure.Data.Repositories;
using CloudAccounting.Web.Endpoints;
using Mapster;
using MapsterMapper;
using FluentValidation;


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

    public static void AddCustomSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen(c =>
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CloudAccounting", Version = "v1" })
        );

    public static void UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CloudAccounting v1"));
    }

    public static void AddInfrastructureDataLayer(this WebApplicationBuilder builder)
    {
        string? connectionString =
             builder.Configuration["ConnectionStrings:OracleConnectionDev"]
                ?? throw new ArgumentNullException("Db connection string is null.");

        builder.Services
            // Infrastructure.Data Layer
            .AddScoped<ICompanyRepository, CompanyRepository>()
            .AddDbContext<CloudAccountingContext>(options =>
            options.UseOracle(connectionString));
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

    public static void AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(ApplicationAssembly.Instance);
        config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateCompanyCommand>, CreateCompanyValidator>();
    }
}
