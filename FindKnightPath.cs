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
        private readonly ILogger _logger;

        public FindKnightPath(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FindKnightPath>();
        }

        [Function("FindKnightPath")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequestData req,
            [SqlInput(commandText: "select * from dbo.Paths where TrackingId = @TrackingId",
                commandType: System.Data.CommandType.Text,
                parameters: "@TrackingId={Query.operationId}",
                connectionStringSetting: "SqlConnectionString")]
            IEnumerable<Path> path)
        {
            var result = path.FirstOrDefault();
            var json = new {
                Starting = result.SourcePosition,
                Ending = result.TargetPosition,
                ShortestPath = result.ShortestPath,
                NumberOfMoves = result.NumberOfMoves,
                OperationId = result.TrackingId
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync(result.ShortestPath);

            return response;
        }
    }
}
