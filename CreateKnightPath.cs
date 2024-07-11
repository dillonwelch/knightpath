using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace KnightPath
{
    public class CreateKnightPath
    {
        private readonly ILogger<CreateKnightPath> _logger;

        public CreateKnightPath(ILogger<CreateKnightPath> logger)
        {
            _logger = logger;
        }

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

        [Function("CreateKnightPath")]
        public async Task<MultiResponse> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try 
            {
                var input = JsonSerializer.Deserialize<CreateKnightPathRequest>(requestBody);
                var trackingId = Guid.NewGuid().ToString();

                // TODO: JSON response?
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                await response.WriteStringAsync(trackingId);

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
            catch (JsonException ex) 
            {
                _logger.LogError("Error deserializing JSON: {Error}", ex.Message);

                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                await response.WriteStringAsync("oh no!");

                return new MultiResponse()
                {
                    HttpResponse = response
                };
            }
        }
    }
}
