using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using CloudAccounting.Web.Extentions;
using CloudAccounting.Web.Middleware;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("starting server.");

    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile("appsettings.Development.json", false, true)
        .AddEnvironmentVariables();

    builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration) // Read configuration from appsettings.json
                .WriteTo.Console() // Log to console for local visibility
                .WriteTo.Seq("http://ubuntu-2404:5341"); // Log to Seq server
        });

    builder.Services.ConfigureAuthentication();
    builder.Services.ConfigureCors();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddMemoryCache();
    builder.Services.AddResponseCaching();
    builder.Services.AddCustomSwagger();
    builder.AddInfrastructureDataLayer();
    builder.Services.AddEndpoints(typeof(Program).Assembly);
    builder.Services.AddMappings();
    builder.Services.AddMediatr();
    builder.Services.RegisterValidators();

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseCustomSwagger();
    }

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    app.UseCors(policyName: "CloudAccounting.Web.Policy");

    app.UseResponseCaching();

    ApiVersionSet apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .ReportApiVersions()
        .Build();

    RouteGroupBuilder versionedGroup = app
        .MapGroup("api/v{version:apiVersion}")
        .WithApiVersionSet(apiVersionSet);

    app.MapEndpoints(versionedGroup);
    app.MapGet("/", () => Results.LocalRedirect("~/swagger"));

    // Define a protected endpoint
    app.MapGet("/secure", [Authorize] () => "This is a secure endpoint that requires authorization.")
       .RequireAuthorization();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush(); // Ensure all buffered logs are written
}

public partial class Program { }
