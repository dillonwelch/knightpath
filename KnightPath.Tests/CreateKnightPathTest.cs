// using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace KnightPath.Tests;
  
[TestFixture]
public class CreateKnightPathTest
{
    [Test]
    public async Task KnightValidTest()
    {
        // var request = new Mock<HttpRequestData>();
        var body = new { Source = "A1", Target = "D5" };
        var json = JsonSerializer.Serialize(body);
        // var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        // request.SetupGet(req => req.Body).Returns(memoryStream);

        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody(json)
            .Build();

        var logger = new NullLogger<CreateKnightPath>();
        CreateKnightPath function = new(logger);
        var response = await function.RunAsync(mockHttpRequest);

        // Assert.That(response, Is.InstanceOf(typeof(OkObjectResult)));

        // var okResult = response as OkObjectResult;
        // var trackingId = okResult?.Value?.ToString();
        // Assert.That(db.StringGet(trackingId).ToString(), Is.EqualTo(json));
    }

    // [Test]
    // public async Task KnightMissingParamsTest()
    // {
    //     var request = new Mock<HttpRequest>();
    //     var body = new { Meow = "A1", Woof = "D5" };
    //     var json = JsonSerializer.Serialize(body);
    //     var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
    //     request.SetupGet(req => req.Body).Returns(memoryStream);

    //     var logger = new NullLogger<CreateKnightPath>();
    //     var function = new CreateKnightPath(logger, _context);
    //     var response = await function.Run(request.Object);

    //     Assert.That(response, Is.InstanceOf(typeof(BadRequestObjectResult)));
    //     var badResult = response as BadRequestObjectResult;
    //     Assert.That(badResult?.Value?.ToString(), Is.EqualTo("Invalid request params."));
    // }

    // // TODO: invalid bad json
    // [Test]
    // public async Task KnightMissingBodyTest()
    // {
    //     var request = new Mock<HttpRequest>();
    //     var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(""));
    //     request.SetupGet(req => req.Body).Returns(memoryStream);

    //     var logger = new NullLogger<CreateKnightPath>();
    //     var function = new CreateKnightPath(logger, _context);
    //     var response = await function.Run(request.Object);

    //     Assert.That(response, Is.InstanceOf(typeof(BadRequestObjectResult)));
    //     var badResult = response as BadRequestObjectResult;
    //     Assert.That(badResult?.Value?.ToString(), Is.EqualTo("Invalid JSON format."));
    // }
}

