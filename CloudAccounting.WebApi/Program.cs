using Microsoft.AspNetCore.HttpLogging;         // To use HttpLoggingFields.
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;                        // To use MapScalarApiReference method.
using CloudAccounting.DataContext;
using CloudAccounting.Repositories.Interface;
using CloudAccounting.Repositories.Repository;
using CloudAccounting.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc.Formatters;        // To use AddUriVersioning method.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUriVersioning();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(defaultScheme: "Bearer").AddJwtBearer();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CloudAccounting.WebApi.Policy",
      policy =>
      {
          policy.WithOrigins("https://localhost:7281");
      });
});

var connectionString = builder.Configuration.GetConnectionString("OracleConnectionDev");

builder.Services.AddDbContext<CloudAccountingContext>(options =>
    options.UseOracle(connectionString));

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddControllers(options =>
{
    WriteLine("Default output formatters:");

    foreach (IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaFormatter = formatter as OutputFormatter;

        if (mediaFormatter is null)
        {
            WriteLine($" {formatter.GetType().Name}");
        }
        else // OutputFormatter class has SupportedMediaTypes.
        {
            WriteLine(" {0}, Media types: {1}",
              arg0: mediaFormatter.GetType().Name,
              arg1: string.Join(", ",
              mediaFormatter.SupportedMediaTypes));
        }
    }
})
.AddXmlDataContractSerializerFormatters()
.AddXmlSerializerFormatters();

builder.Services.AddOpenApi();

builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

builder.Services.AddHttpLogging(options =>
{
    options.RequestHeaders.Add("Origin");
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096; // Default is 32k.
    options.ResponseBodyLogLimit = 4096; // Default is 32k.
});

var app = builder.Build();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(policyName: "CloudAccounting.WebApi.Policy");

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
