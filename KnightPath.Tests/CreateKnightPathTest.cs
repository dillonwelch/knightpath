// using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq; // TODO: Remove?

namespace KnightPath.Tests;
  
[TestFixture]
public class CreateKnightPathTest
{
    [Test]
    public async Task CreateKnightPathSuccessTest()
    {
        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{\"Source\": \"A1\", \"Target\": \"D5\"}")
            .Build();

        var logger = new NullLogger<CreateKnightPath>();
        CreateKnightPath function = new(logger);
        var response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);

        Assert.That(response.HttpResponse, Is.InstanceOf(typeof(HttpResponseData)));
        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Message, Is.Not.Null);
        });

        Assert.Multiple(() =>
        {
            Assert.That(response.Message.Source, Is.EqualTo("A1"));
            Assert.That(response.Message.Target, Is.EqualTo("D5"));
            Assert.That(Guid.TryParse(response.Message.TrackingId, out Guid val));
        });

    }

    [Test]
    public async Task KnightMissingParamsTest()
    {
        var body = new { Meow = "A1", Woof = "D5" };
        var json = JsonSerializer.Serialize(body);

        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody(json)
            .Build();

        var logger = new NullLogger<CreateKnightPath>();
        CreateKnightPath function = new(logger);
        var response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);

        Assert.That(response.HttpResponse, Is.InstanceOf(typeof(HttpResponseData)));
        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Message, Is.Null);
        });

    }

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

