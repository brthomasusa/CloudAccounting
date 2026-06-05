using CloudAccounting.SharedKernel.Utilities;

namespace CloudAccounting.Web.Extentions
{
    public static class ResultExtensions
    {
        extension(Result result)
        {
            public IResult ToBadRequestProblemDetails()
            {
                return result.IsSuccess
                    ? throw new InvalidOperationException()
                    : TypedResults.Problem(
                        detail: result.Error.Message,
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Bad Request",
                        type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        extensions: new Dictionary<string, object?>
                        {
                            { "errors", new[] { result.Error } }
                        });
            }

            public IResult ToNotFoundProblemDetails()
            {
                return result.IsSuccess
                    ? throw new InvalidOperationException()
                    : TypedResults.Problem(
                        detail: result.Error.Message,
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Not Found",
                        type: "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        extensions: new Dictionary<string, object?>
                        {
                            { "errors", new[] { result.Error } }
                        });
            }

            public IResult ToInternalServerErrorProblemDetails(string errorMessage)
            {
                return result.IsSuccess
                    ? throw new InvalidOperationException()
                    : TypedResults.Problem(
                        detail: errorMessage,
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Internal Server Error",
                        type: "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        extensions: new Dictionary<string, object?>
                        {
                            { "errors", new[] { errorMessage } }
                        });
            }
        }
    }
}