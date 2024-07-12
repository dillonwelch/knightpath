namespace KnightPath
{
    public class Path
    {
        public int PathId { get; set; }
        public required Guid TrackingId { get; set; }
        public required string ShortestPath { get; set; }
        public required int NumberOfMoves { get; set; }

        public required string SourcePosition { get; set; }
        public required string TargetPosition { get; set; }
    }
}
