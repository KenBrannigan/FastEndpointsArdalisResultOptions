using Ardalis.Result;
using FastEndpoints;
using FastEndpointsArdalisResultOptions.Endpoints.Extensions;
using FastEndpointsArdalisResultOptions.Endpoints.Records;
using FastEndpointsArdalisResultOptions.Endpoints.Responses;

namespace FastEndpointsArdalisResultOptions.Endpoints
{
    public class Option1 : EndpointWithoutRequest<DataResponse>
    {
        public override void Configure()
        {
            Get("option1");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            // Use MediatR to get a DataRecord from a Use Case wrapped in an Ardalis Result
            var result = Result.Success(new DataRecord(1, "Option 1"));

            // Use an extension method to call the correct SendXXX function provider by FastEndpoints
            // No way to verify that DataResponse defined as the type listed above is actually the type returned
            // Could just rely on unit/integration tests to verify the correct response type is returned
            await this.SendArdalisResultAsync(
                result,
                r => new DataResponse(r.Value.Id, r.Value.Name),
                ct);
        }
    }
}
