using Ardalis.Result;
using FastEndpoints;
using System.Net;

namespace FastEndpointsArdalisResultOptions.Endpoints.Extensions
{
    public static class IEndpointExtension
    {
        public static async Task SendArdalisResultAsync<TResult, TResponse>(this IEndpoint endpoint, TResult result, Func<TResult, TResponse> mapper, CancellationToken cancellation = default) where TResult : class, Ardalis.Result.IResult
        {
            switch (result.Status)
            {
                case ResultStatus.Ok:
                    await endpoint.HttpContext.Response
                        .SendOkAsync(mapper(result), cancellation: cancellation)
                        .ConfigureAwait(false);
                    break;

                case ResultStatus.Forbidden:
                    await endpoint.HttpContext.Response
                        .SendForbiddenAsync(cancellation)
                        .ConfigureAwait(false);
                    break;

                case ResultStatus.Conflict:
                case ResultStatus.CriticalError:
                case ResultStatus.Error:
                case ResultStatus.Invalid:
                case ResultStatus.Unavailable:
                    HttpStatusCode httpStatusCode = result.Status switch
                    {
                        ResultStatus.Conflict => HttpStatusCode.Conflict,
                        ResultStatus.CriticalError => HttpStatusCode.InternalServerError,
                        ResultStatus.Error => HttpStatusCode.UnprocessableEntity,
                        ResultStatus.Unavailable => HttpStatusCode.ServiceUnavailable,
                        _ => HttpStatusCode.BadRequest,
                    };

                    result.ValidationErrors.ForEach(e =>
                        endpoint.ValidationFailures.Add(new(e.Identifier, e.ErrorMessage)));

                    await endpoint.HttpContext.Response
                        .SendErrorsAsync(endpoint.ValidationFailures, (int)httpStatusCode, cancellation: cancellation)
                        .ConfigureAwait(false);
                    break;

                case ResultStatus.NotFound:
                    await endpoint.HttpContext.Response
                        .SendNotFoundAsync(cancellation)
                        .ConfigureAwait(false);
                    break;

                case ResultStatus.Unauthorized:
                    await endpoint.HttpContext.Response
                        .SendUnauthorizedAsync(cancellation)
                        .ConfigureAwait(false);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result.Status, null);
            }
        }
    }
}
