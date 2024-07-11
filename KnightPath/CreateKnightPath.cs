using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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

        public CreateKnightPath(ILogger<CreateKnightPath> logger)
        {
            _logger = logger;
        }

        [Function("CreateKnightPath")]
        public async Task<MultiResponse> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            ArgumentNullException.ThrowIfNull(req);

            StreamReader reader = new(req.Body);
            string requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
            reader.Dispose();

            try
            {
                var input = JsonSerializer.Deserialize<CreateKnightPathRequest>(requestBody);
                // TODO: Validate it matches the A1 pattern
                ArgumentNullException.ThrowIfNullOrWhiteSpace(input?.Source);
                ArgumentNullException.ThrowIfNullOrWhiteSpace(input?.Target);

                var trackingId = Guid.NewGuid().ToString();

                // TODO: JSON response?
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                await response.WriteStringAsync(trackingId).ConfigureAwait(false);

                CreateKnightPathQueueMessage message = new()
                {
                    TrackingId = trackingId,
                    Source = input.Source,
                    Target = input.Target
                };

                return new MultiResponse()
                {
                    Message = message,
                    HttpResponse = response
                };
            }
            catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is JsonException)
            {
                // NOTE: Unclear how to best implement the solution.
                # pragma warning disable CA1848
                _logger.LogError("Error processing request body: {Error}", e.Message);
                # pragma warning restore CA1848

                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                await response.WriteStringAsync("Error processing request body!").ConfigureAwait(false);

                return new MultiResponse()
                {
                    HttpResponse = response
                };
            }
        }
    }
}
