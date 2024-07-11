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
            public string? Source { get; set; }
            public string? Target { get; set; }
        }

        public class MultiResponse
        {
            [QueueOutput("knightpathqueue")]
            public string? Message { get; set; }
            public required HttpResponseData HttpResponse { get; set; }
        }

        [Function("CreateKnightPath")]
        public async Task<MultiResponse> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonSerializer.Deserialize<CreateKnightPathRequest>(requestBody);
            var trackingId = Guid.NewGuid().ToString();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync(trackingId);

            return new MultiResponse() 
            {
                Message = trackingId,
                HttpResponse = response
            };
        }
    }
}
