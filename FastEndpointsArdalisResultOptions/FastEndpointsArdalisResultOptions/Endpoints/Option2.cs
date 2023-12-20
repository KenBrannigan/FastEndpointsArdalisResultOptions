using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using FastEndpointsArdalisResultOptions.Endpoints.Records;
using FastEndpointsArdalisResultOptions.Endpoints.Responses;

namespace FastEndpointsArdalisResultOptions.Endpoints
{
    public class Option2 : EndpointWithoutRequest<Microsoft.AspNetCore.Http.IResult>
    {
        public override void Configure()
        {
            Get("option2");
            AllowAnonymous();
        }

        public override async Task<Microsoft.AspNetCore.Http.IResult> ExecuteAsync(CancellationToken ct)
        {
            await Task.CompletedTask;

            // Use MediatR to get a DataRecord from a Use Case wrapped in an Ardalis Result
            var result = Result.Success(new DataRecord(2, "Option 2"));

            // Set the response type above to Microsoft.AspNetCore.Http.IResult
            // Use the ToMinimalApiResult extension method to convert the Ardalis Result to a Microsoft.AspNetCore.Http.IResult
            // Better than Option 1 because the response type is verified at compile time but it is not the DataResponse type I want
            // Could just rely on unit/integration tests to verify the correct response type is returned
            return result
                .Map(r => new DataResponse(r.Id, r.Name))
                .ToMinimalApiResult();
        }
    }
}
