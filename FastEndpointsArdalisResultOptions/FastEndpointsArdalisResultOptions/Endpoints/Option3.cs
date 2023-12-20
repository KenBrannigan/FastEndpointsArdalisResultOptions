using Ardalis.Result;
using FastEndpointsArdalisResultOptions.Endpoints.Bases;
using FastEndpointsArdalisResultOptions.Endpoints.Records;
using FastEndpointsArdalisResultOptions.Endpoints.Responses;

namespace FastEndpointsArdalisResultOptions.Endpoints
{
    public class Option3 : EndpointWithoutRequestBase<DataResponse>
    {
        public override void Configure()
        {
            Get("option3");
            AllowAnonymous();
        }

        public override async Task<Result<DataResponse>> ProcessAsync(CancellationToken ct)
        {
            await Task.CompletedTask;

            // Use MediatR to get a DataRecord from a Use Case wrapped in an Ardalis Result
            var result = Result.Success(new DataRecord(3, "Option 3"));

            // Using the custom base class allows me to set the proper response type so compile time checks can be performed
            return result
                .Map(r => new DataResponse(r.Id, r.Name));
        }
    }
}
