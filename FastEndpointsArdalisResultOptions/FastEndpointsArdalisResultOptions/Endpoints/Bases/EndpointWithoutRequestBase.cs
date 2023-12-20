using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;

namespace FastEndpointsArdalisResultOptions.Endpoints.Bases
{
    public abstract class EndpointWithoutRequestBase<TResponse> : EndpointWithoutRequest<Microsoft.AspNetCore.Http.IResult> where TResponse : notnull
    {
        public sealed override async Task<Microsoft.AspNetCore.Http.IResult> ExecuteAsync(CancellationToken ct)
        {
            Result<TResponse> response = await ProcessAsync(ct)
                .ConfigureAwait(false);

            return response
                .ToMinimalApiResult();
        }

        public abstract Task<Result<TResponse>> ProcessAsync(CancellationToken ct);
    }
}
