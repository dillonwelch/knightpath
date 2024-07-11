using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
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
        [SqlOutput("dbo.Paths", connectionStringSetting: "SqlConnectionString")]
        public Path Run([QueueTrigger("knightpathqueue")] QueueMessage message)
        {
            // TODO: Catch errors
            ArgumentNullException.ThrowIfNull(message);
            var input = JsonSerializer.Deserialize<CreateKnightPathQueueMessage>(message.MessageText);
            ArgumentNullException.ThrowIfNull(input);
            var shortestPath = ShortestPathCalculator.CalculateShortestPath(input.Source, input.Target);
            var stringPath = String.Join(":", shortestPath);

            var rawTrackingId = input.TrackingId;
            if (!Guid.TryParse(rawTrackingId, out Guid trackingId))
            {
                // TODO: How to handle
                // NOTE: Unclear how to best implement the solution.
                # pragma warning disable CA1848
                _logger.LogError("Invalid id format: {RawTrackingId}", rawTrackingId);
                # pragma warning restore CA1848
                // return new BadRequestObjectResult("Invalid id format.");
            }

            return new Path() {
                SourcePosition = input.Source,
                TargetPosition = input.Target,
                TrackingId = trackingId,
                NumberOfMoves = shortestPath.Count - 1,
                ShortestPath = stringPath
            };
        }
    }
}
