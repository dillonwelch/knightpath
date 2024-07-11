using System.Text.Json;
using Azure.Storage.Queues.Models;
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
        FormatException e = Assert.Throws<FormatException>(() => function.Run(message));
        Assert.That(e.Message, Is.EqualTo("Unrecognized Guid format."));
    }

    [Test]
    public void CalculateShortestPathMissingKeysTest()
    {
        string messageText = "{ \"Meow\": \"123\", \"Woof\": \"A1\", \"Ribbit\": \"D5\" }";
        QueueMessage message = QueuesModelFactory.QueueMessage(
          messageId: "id",
          popReceipt: "pr",
          body: BinaryData.FromString(messageText),
          dequeueCount: 0);

        var logger = new NullLogger<CalculateShortestPath>();
        CalculateShortestPath function = new(logger);
        JsonException e = Assert.Throws<JsonException>(() => function.Run(message));
        Assert.That(e.Message, Is.EqualTo("JSON deserialization for type 'KnightPath.CreateKnightPathQueueMessage' was missing required properties, including the following: TrackingId, Source, Target"));
    }

    [Test]
    public void CalculateShortestPathEmptySourceTest()
    {
        string messageText = "{ \"TrackingId\": \"123\", \"Source\": \"\", \"Target\": \"D5\" }";
        QueueMessage message = QueuesModelFactory.QueueMessage(
          messageId: "id",
          popReceipt: "pr",
          body: BinaryData.FromString(messageText),
          dequeueCount: 0);

        var logger = new NullLogger<CalculateShortestPath>();
        CalculateShortestPath function = new(logger);
        ArgumentException e = Assert.Throws<ArgumentException>(() => function.Run(message));
        Assert.That(e.Message, Is.EqualTo("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'position')"));
    }

    [Test]
    public void CalculateShortestPathEmptyTargetTest()
    {
        string messageText = "{ \"TrackingId\": \"123\", \"Source\": \"A1\", \"Target\": \"\" }";
        QueueMessage message = QueuesModelFactory.QueueMessage(
          messageId: "id",
          popReceipt: "pr",
          body: BinaryData.FromString(messageText),
          dequeueCount: 0);

        var logger = new NullLogger<CalculateShortestPath>();
        CalculateShortestPath function = new(logger);
        ArgumentException e = Assert.Throws<ArgumentException>(() => function.Run(message));
        Assert.That(e.Message, Is.EqualTo("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'position')"));
    }

    [Test]
    public void CalculateShortestPathInvalidJsonTest()
    {
        string messageText = "{ Hey I am not valid }";
        QueueMessage message = QueuesModelFactory.QueueMessage(
          messageId: "id",
          popReceipt: "pr",
          body: BinaryData.FromString(messageText),
          dequeueCount: 0);

        var logger = new NullLogger<CalculateShortestPath>();
        CalculateShortestPath function = new(logger);
        JsonException e = Assert.Throws<JsonException>(() => function.Run(message));
        Assert.That(e.Message, Is.EqualTo("'H' is an invalid start of a property name. Expected a '\"'. Path: $ | LineNumber: 0 | BytePositionInLine: 2."));
    }
}