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

        [Function("CreateKnightPath")]
        [QueueOutput("knightpathqueue")]    
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            return new OkObjectResult("Welcome to Azure Functions: " + requestBody);
        }
    }
}
