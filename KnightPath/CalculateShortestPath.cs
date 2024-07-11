using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using static KnightPath.ChessBoard;

namespace KnightPath
{
    public class CalculateShortestPath
    {
        private readonly ILogger<CalculateShortestPath> _logger;

        public CalculateShortestPath(ILogger<CalculateShortestPath> logger)
        {
            _logger = logger;
        }

        [Function(nameof(CalculateShortestPath))]
        [SqlOutput("dbo.Paths", connectionStringSetting: "SqlConnectionString")]
        public Path Run([QueueTrigger("knightpathqueue")] QueueMessage message)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(message);
                var input = JsonSerializer.Deserialize<CreateKnightPathQueueMessage>(message.MessageText);
                ArgumentNullException.ThrowIfNull(input);
                ValidatePosition(input.Source);
                ValidatePosition(input.Target);

                if (Guid.TryParse(input.TrackingId, out Guid trackingId))
                {
                    var shortestPath = ShortestPathCalculator.CalculateShortestPath(input.Source, input.Target);
                    var stringPath = String.Join(":", shortestPath);

                    return new Path() {
                        SourcePosition = input.Source,
                        TargetPosition = input.Target,
                        TrackingId = trackingId,
                        NumberOfMoves = shortestPath.Count - 1,
                        ShortestPath = stringPath
                    };
                }
                else
                {
                    throw new ArgumentException("TrackingId '{RawTrackingId}' is not a Guid.", input.TrackingId);
                }
            }
            // Note: Unsure if exception throwing is the "right" way to handle these errors.
            catch (Exception e) 
            {
                // NOTE: Unclear how to best implement the solution.
                # pragma warning disable CA1848
                _logger.LogError("Error when processing QueueMessage: {Error}", e.Message);
                #pragma warning restore CA1848

                throw;
            }
        }
    }
}