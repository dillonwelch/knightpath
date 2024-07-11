using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging.Abstractions;
using static KnightPath.Tests.TestHelpers;

namespace KnightPath.Tests;
  
[TestFixture]
public class FindKnightPathTest
{
    [Test]
    public async Task FindKnightPathSuccessTest()
    {
        Guid operationId = Guid.NewGuid();

        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("")
            .Build();

        Path path = new () {
          SourcePosition = "A1",
          TargetPosition = "D5",
          ShortestPath = "A1:C2:B4:D5",
          NumberOfMoves = 3,
          TrackingId = operationId
        };

        var logger = new NullLogger<FindKnightPath>();
        FindKnightPath function = new(logger);
        var response = await FindKnightPath.RunAsync(mockHttpRequest, [path]).ConfigureAwait(false);

        Assert.That(response, Is.InstanceOf(typeof(HttpResponseData)));

        var streamText = await ReadBody(response.Body).ConfigureAwait(false);
        string json = "{\"Starting\":\"A1\",\"Ending\":\"D5\",\"ShortestPath\":\"A1:C2:B4:D5\",\"NumberOfMoves\":3,\"OperationId\":\"" + operationId.ToString() + "\"}";

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(streamText, Is.EqualTo(json));
        });
    }

    [Test]
    public async Task FindKnightPathNotFoundTest()
    {
        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("")
            .Build();

        var logger = new NullLogger<FindKnightPath>();
        FindKnightPath function = new(logger);
        var response = await FindKnightPath.RunAsync(mockHttpRequest, []).ConfigureAwait(false);

        Assert.That(response, Is.InstanceOf(typeof(HttpResponseData)));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}