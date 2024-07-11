using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KnightPath
{
    public class FindKnightPath
    {
        private readonly ILogger<FindKnightPath> _logger;

        public FindKnightPath(ILogger<FindKnightPath> logger)
        {
            _logger = logger;
        }

        [Function("FindKnightPath")]
        public static async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "knightpath")]
            HttpRequestData req,
            [SqlInput(commandText: "select * from dbo.Paths where TrackingId = @TrackingId",
                commandType: System.Data.CommandType.Text,
                parameters: "@TrackingId={Query.operationId}",
                connectionStringSetting: "SqlConnectionString")]
            IEnumerable<Path> path)
        {
            Path? result = path.FirstOrDefault();

            if (result is not null)
            {
                var json = new {
                    Starting = result.SourcePosition,
                    Ending = result.TargetPosition,
                    result.ShortestPath,
                    result.NumberOfMoves,
                    OperationId = result.TrackingId
                };

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(json).ConfigureAwait(false);

                return response;
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
