using System.Net;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace KnightPath.Tests;
  
[TestFixture]
public class CalculateShortestPathTest
{
    [Test]
    public void CalculateShortestPathSuccessTest()
    {
        Guid trackingId = new();
        string messageText = "{ \"TrackingId\": \"" + trackingId.ToString() + "\", \"Source\": \"A1\", \"Target\": \"D5\" }";
        QueueMessage message = QueuesModelFactory.QueueMessage(
          messageId: "id",
          popReceipt: "pr",
          body: BinaryData.FromString(messageText),
          dequeueCount: 0);

        var logger = new NullLogger<CalculateShortestPath>();
        CalculateShortestPath function = new(logger);
        var response = function.Run(message);

        Assert.That(response, Is.InstanceOf(typeof(Path)));
        Assert.Multiple(() =>
        {
            Assert.That(response.SourcePosition, Is.EqualTo("A1"));
            Assert.That(response.TargetPosition, Is.EqualTo("D5"));
            Assert.That(response.NumberOfMoves, Is.EqualTo(3));
            Assert.That(response.TrackingId, Is.EqualTo(trackingId));
            Assert.That(response.ShortestPath, Is.EqualTo("A1:C2:B4:D5"));
        });
    }

    [Test]
    public void CalculateShortestPathInvalidTrackingIdTest()
    {
        string messageText = "{ \"TrackingId\": \"123\", \"Source\": \"A1\", \"Target\": \"D5\" }";
        QueueMessage message = QueuesModelFactory.QueueMessage(
          messageId: "id",
          popReceipt: "pr",
          body: BinaryData.FromString(messageText),
          dequeueCount: 0);

        var logger = new NullLogger<CalculateShortestPath>();
        CalculateShortestPath function = new(logger);
        Assert.Throws<ArgumentException>(() => function.Run(message));
    }
}