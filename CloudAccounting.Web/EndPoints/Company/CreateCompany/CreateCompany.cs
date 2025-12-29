using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Application.UseCases.GetCompanyById;
using CloudAccounting.SharedKernel.Utilities;
using CloudAccounting.Web.Endpoints;
using CloudAccounting.Web.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CloudAccounting.Web.EndPoints.Company.CreateCompany
{
    public class CreateCompany : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("companies/", CreateCompanyFromCommand);
        }

        public static async Task<IResult> CreateCompanyFromCommand([FromBody] CreateCompanyCommand command, ISender sender, ILogger<GetCompanyById> logger)
        {
            Result<int>? result = null;

            try
            {
                result = await sender.Send(command);

                if (result.IsSuccess)
                {
                    Result<CompanyDetailVm> getCompanyResult =
                        await sender.Send(new GetCompanyByIdQuery(CompanyCode: result.Value));

                    return Results.CreatedAtRoute(
                        routeName: "GetCompanyByCompanyCode",
                        routeValues: new { companyCode = result.Value },
                        getCompanyResult.Value
                    );
                }

                string msg = result.Error.Message;
                logger.LogWarning("There was a problem creating the new company: {ERROR}", result.Error.Message);
                return Results.NotFound(msg);

            }
            catch (Exception ex)
            {
                string msg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", msg);
                return result!.ToInternalServerErrorProblemDetails(msg);
            }
        }
    }
}