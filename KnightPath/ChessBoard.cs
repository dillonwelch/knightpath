namespace KnightPath
{
    public static class ChessBoard
    {
        public static readonly Dictionary<string, int> BoardMapping = new Dictionary<string, int>()
        {
            { "A", 0 }, { "B", 1 }, { "C", 2 }, { "D", 3 }, { "E", 4 }, { "F", 5 }, { "G", 6 }, { "H", 7 },
        };
        // TODO: build warn
        public static readonly List<string> BoardMappingKeys = [.. BoardMapping.Keys];

        public const int Rows = 8, Colunns = 8;
    }
}