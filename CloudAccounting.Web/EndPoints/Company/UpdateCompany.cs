using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Application.UseCases.UpdateCompany;
using CloudAccounting.SharedKernel.Utilities;
using CloudAccounting.Web.Endpoints;
using CloudAccounting.Web.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CloudAccounting.Web.EndPoints.Company
{
    public class UpdateCompany : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("companies/", UpdateCompanyFromCommand);
        }

        public static async Task<IResult> UpdateCompanyFromCommand([FromBody] UpdateCompanyCommand command, ISender sender, ILogger<UpdateCompany> logger)
        {
            Result<CompanyDetailVm>? result = null;

            try
            {
                result = await sender.Send(command);

                if (result.IsSuccess)
                {
                    return Results.CreatedAtRoute(
                        routeName: "GetCompanyByCompanyCode",
                        routeValues: new { companyCode = result.Value.CompanyCode },
                        result.Value
                    );
                }

                string msg = result.Error.Message;
                logger.LogWarning("There was a problem updating the new company: {ERROR}", result.Error.Message);
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