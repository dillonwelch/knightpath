using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
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

        [Function("CreateKnightPath")]
        [QueueOutput("knightpathqueue")]    
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonSerializer.Deserialize<CreateKnightPathRequest>(requestBody);
            var trackingId = Guid.NewGuid().ToString();

            return new OkObjectResult(new {
                TrackingId = trackingId,
                Source = input.Source,
                Target = input.Target
            });
        }
    }
}
