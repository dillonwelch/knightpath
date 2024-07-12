namespace KnightPath
{
  // TODO: How to set this up in the DB
  // [Index(nameof(TrackingId), IsUnique = true)]
  public class Path
  {
    // TODO: Comments
    public int PathId { get; set; }
    public required Guid TrackingId { get; set; }
    public required string ShortestPath { get; set; }
    public required int NumberOfMoves { get; set; }
    // TODO: validate inputs
    public required string SourcePosition { get; set; }
    public required string TargetPosition { get; set; }
  }
}
