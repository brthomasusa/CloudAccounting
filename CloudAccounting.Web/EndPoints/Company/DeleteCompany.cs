using CloudAccounting.Application.UseCases.DeleteCompany;
using CloudAccounting.SharedKernel.Utilities;
using CloudAccounting.Web.Endpoints;
using CloudAccounting.Web.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CloudAccounting.Web.EndPoints.Company
{
    public class DeleteCompany : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("companies/", DeleteCompanyFromCommand);
        }

        public static async Task<IResult> DeleteCompanyFromCommand([FromBody] DeleteCompanyCommand command, ISender sender, ILogger<DeleteCompany> logger)
        {
            Result<MediatR.Unit>? result = null;

            try
            {
                result = await sender.Send(command);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                string msg = result.Error.Message;
                logger.LogWarning("There was a problem deleting the company '{COMPANYCODE}': {ERROR}", command.CompanyCode, result.Error.Message);
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