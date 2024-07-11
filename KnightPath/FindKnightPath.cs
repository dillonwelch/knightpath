using System;
using System.Collections.Generic;
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
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequestData req,
            [SqlInput(commandText: "select * from dbo.Paths where TrackingId = @TrackingId",
                commandType: System.Data.CommandType.Text,
                parameters: "@TrackingId={Query.operationId}",
                connectionStringSetting: "SqlConnectionString")]
            IEnumerable<Path> path)
        {
            var result = path.FirstOrDefault();

            if (result is not null)
            {
                var json = new {
                    Starting = result.SourcePosition,
                    Ending = result.TargetPosition,
                    ShortestPath = result.ShortestPath,
                    NumberOfMoves = result.NumberOfMoves,
                    OperationId = result.TrackingId
                };

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(json).ConfigureAwait(false);

                return response;
            }
            else
            {
                var response = req.CreateResponse(HttpStatusCode.NotFound);
                return response;
            }
        }
    }
}
