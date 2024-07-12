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
            _logger.LogInformation("Starting to calculate...");
            try
            {
                ArgumentNullException.ThrowIfNull(message);
                CreateKnightPathQueueMessage? input = JsonSerializer.Deserialize<CreateKnightPathQueueMessage>(message.MessageText);
                ArgumentNullException.ThrowIfNull(input);
                ValidatePosition(input.Source);
                ValidatePosition(input.Target);

                Guid trackingId = Guid.Parse(input.TrackingId);
                IList<string> shortestPath = ShortestPathCalculator.CalculateShortestPath(input.Source, input.Target);
                string stringPath = string.Join(":", shortestPath);

                _logger.LogInformation("Finished calculating...");

                return new Path() {
                    SourcePosition = input.Source,
                    TargetPosition = input.Target,
                    TrackingId = trackingId,
                    NumberOfMoves = shortestPath.Count - 1,
                    ShortestPath = stringPath
                };
            }
            // NOTE: Unsure if exception rethrowing is the "right" way to handle these errors. 
            // Do we need to manually log this, or is it logged already?
            // What will this do to queue processing? Wil lit repeat endlessly?
            catch (Exception e) 
            {
                // NOTE: Unclear how to best implement the solution.
                # pragma warning disable CA1848
                _logger.LogError("Error when processing QueueMessage: {Error}", e.Message);
                # pragma warning restore CA1848

                throw;
            }
        }
    }
}