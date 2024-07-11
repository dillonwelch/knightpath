using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace KnightPath.Tests;
  
[TestFixture]
public class FindKnightPathTest
{
    // NOTE: I would love to be testing the response body content but I can't figure out how :(
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
        var response = await function.RunAsync(mockHttpRequest, [path]).ConfigureAwait(false);

        Assert.That(response, Is.InstanceOf(typeof(HttpResponseData)));

        response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(response.Body);
        var streamText = await reader.ReadToEndAsync().ConfigureAwait(false);
        reader.Dispose();
        string json = "{\"Starting\":\"A1\",\"Ending\":\"D5\",\"ShortestPath\":\"A1:C2:B4:D5\",\"NumberOfMoves\":3,\"OperationId\":\"" + operationId.ToString() + "\"}";

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(streamText, Is.EqualTo(json));
        });

        // Assert.Multiple(() =>
        // {
        //     Assert.That(response.Message.Source, Is.EqualTo("A1"));
        //     Assert.That(response.Message.Target, Is.EqualTo("D5"));
        //     Assert.That(Guid.TryParse(response.Message.TrackingId, out Guid val));
        // });

    }
}