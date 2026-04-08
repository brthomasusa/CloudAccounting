using System.Text;
using CloudAccounting.Application;
using CloudAccounting.Infrastructure.Data.Data;
using CloudAccounting.Infrastructure.Data.Options;
using CloudAccounting.Web.Authorization;
using CloudAccounting.Web.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile("appsettings.Development.json", false, true)
        .AddEnvironmentVariables();

    builder.Services.Configure<JwtOptions>(
        builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));

    builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration) // Read configuration from appsettings.json
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341");
        });

    builder.Services.AddOpenApi();
    builder.Services.ConfigureCors();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddEndpoints(typeof(Program).Assembly);
    builder.Services.AddMemoryCache();
    builder.Services.AddResponseCaching();
    builder.Services.AddSwaggerGenAuth();

    builder.Services.AddDbContext<AppDbContext>(options =>
         options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
                .UseSqlServer(builder.Configuration.GetConnectionString("CloudAcctgTest"))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]!))
        };
    });

    builder.Services.AddAuthorization();
    builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicPolicyProvider>();
    builder.Services.AddSingleton<IAuthorizationHandler, DynamicRoleHanlder>();

    builder.AddRepositoriesAndDomainServices();
    builder.Services.AddMediatr();
    builder.Services.RegisterValidators();
    builder.Services.AddMappings();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseHttpsRedirection();
    app.UseResponseCaching();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors(policyName: "CloudAccounting.Web.Policy");
    app.MapEndpoints();

    app.MapGet("/", () => Results.LocalRedirect("~/swagger"));

    Log.Information("Starting web host");

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException && ex.Source != "Microsoft.EntityFrameworkCore.Design")
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
