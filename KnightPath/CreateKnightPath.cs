using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using static KnightPath.ChessBoard;

namespace KnightPath
{
    public class CreateKnightPathRequest
    {
        public required string Source { get; set; }
        public required string Target { get; set; }
    }

    public class CreateKnightPathQueueMessage
    {
        public required string TrackingId { get; set; }
        public required string Source { get; set; }
        public required string Target { get; set; }
    }

    public class MultiResponse
    {
        [QueueOutput("knightpathqueue")]
        public CreateKnightPathQueueMessage? Message { get; set; }
        public required HttpResponseData HttpResponse { get; set; }
    }

    public class CreateKnightPath
    {
        private readonly ILogger<CreateKnightPath> _logger;

        private static readonly JsonSerializerOptions options =
            new() { PropertyNameCaseInsensitive = true };

        public CreateKnightPath(ILogger<CreateKnightPath> logger)
        {
            _logger = logger;
        }

        [Function("CreateKnightPath")]
        public async Task<MultiResponse> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "knightpath")]
                HttpRequestData req
        )
        {
            ArgumentNullException.ThrowIfNull(req);

            StreamReader reader = new(req.Body);
            string requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
            reader.Dispose();

            try
            {
                CreateKnightPathRequest? input =
                    JsonSerializer.Deserialize<CreateKnightPathRequest>(requestBody, options);
                ArgumentNullException.ThrowIfNull(input);
                ValidatePosition(input.Source);
                ValidatePosition(input.Target);

                string trackingId = Guid.NewGuid().ToString();

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                await response.WriteStringAsync(trackingId).ConfigureAwait(false);

                CreateKnightPathQueueMessage message =
                    new()
                    {
                        TrackingId = trackingId,
                        Source = input.Source,
                        Target = input.Target
                    };

                return new MultiResponse() { Message = message, HttpResponse = response };
            }
            catch (Exception e)
                when (e is ArgumentException || e is ArgumentNullException || e is JsonException)
            {
                // NOTE: Unclear how to best implement the solution.
#pragma warning disable CA1848
                _logger.LogError("Error processing request body: {Error}", e.Message);
#pragma warning restore CA1848

                HttpResponseData response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                // NOTE: It would be nice to have different responses for different validation errors.
                await response
                    .WriteStringAsync("Error processing request body!")
                    .ConfigureAwait(false);

                return new MultiResponse() { HttpResponse = response };
            }
        }
    }
}
