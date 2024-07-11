using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KnightPath
{
    public class CalculateShortestPath
    {
        private readonly ILogger<CalculateShortestPath> _logger;

        public CalculateShortestPath(ILogger<CalculateShortestPath> logger)
        {
            _logger = logger;
        }

        // TODO: Move to Common
        public class CreateKnightPathQueueMessage
        {
            public required string TrackingId { get; set; }
            public required string Source { get; set; }
            public required string Target { get; set; }
        }

        [Function(nameof(CalculateShortestPath))]
        public void Run([QueueTrigger("knightpathqueue")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            var input = JsonSerializer.Deserialize<CreateKnightPathQueueMessage>(message.MessageText);

            _logger.LogInformation("Source {Source} and Target {Target} and Tracking ID {TrackingId}", input.Source, input.Target, input.TrackingId);

            var shortestPath = ShortestPathCalculator.CalculateShortestPath(input.Source, input.Target);
            var stringPath = String.Join(":", shortestPath);

            _logger.LogInformation("Shortest path is '{ShortestPath}'", stringPath);
        }
    }
}
