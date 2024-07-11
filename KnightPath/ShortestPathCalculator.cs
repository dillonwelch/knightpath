namespace KnightPath
{
    public static class ShortestPathCalculator
    {
        static readonly Dictionary<string, int> boardMapping = new Dictionary<string, int>()
      {
          { "A", 0 }, { "B", 1 }, { "C", 2 }, { "D", 3 }, { "E", 4 }, { "F", 5 }, { "G", 6 }, { "H", 7 },
      };
        static readonly List<string> boardMappingKeys = [.. boardMapping.Keys];
        static readonly int[] dx = [-2, -1, 1, 2, -2, -1, 1, 2];
        static readonly int[] dy = [-1, -2, -2, -1, 1, 2, 2, 1];

        const int ROWS = 8, COLUMNS = 8;

        static string MoveListString(int positionX, int positionY)
        {
            return $"{boardMappingKeys[positionX]}{(positionY + 1)}";
        }

        static bool OnBoard(int positionX, int positionY)
        {
            if (positionX >= 0 && positionY >= 0 && positionX < ROWS && positionY < COLUMNS)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static IList<string> CalculateShortestPath(string starting, string ending)
        {
            // TODO: How to validate A1 vs 99
            ArgumentNullException.ThrowIfNull(starting);
            ArgumentNullException.ThrowIfNull(ending);

            int startingX = boardMapping[starting[0].ToString()];
            int startingY = (int)(Char.GetNumericValue(starting[1]) - 1);
            int endingX = boardMapping[ending[0].ToString()];
            int endingY = (int)(Char.GetNumericValue(ending[1]) - 1);

            if (!OnBoard(startingX, startingY))
            {
                throw new ArgumentException("Starting position out of bounds.");
            }
            if (!OnBoard(endingX, endingY))
            {
                throw new ArgumentException("Ending position out of bounds.");
            }

            var queue = new Queue<int[]>();
            queue.Enqueue([startingX, startingY]);

            Dictionary<int, Dictionary<int, List<string>>> moveList = [];
            for (int row = 0; row < ROWS; row++)
            {
                moveList[row] = [];
            }
            moveList[startingX][startingY] = [MoveListString(startingX, startingY)];

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentX = current[0];
                var currentY = current[1];

                if (currentX == endingX && currentY == endingY)
                {
                    return moveList[currentX][currentY];
                }

                for (int position = 0; position < 8; position++)
                {
                    var newX = currentX + dx[position];
                    var newY = currentY + dy[position];
                    if (OnBoard(newX, newY) && !moveList[newX].ContainsKey(newY))
                    {
                        moveList[newX][newY] = new List<string>(moveList[currentX][currentY])
                      {
                          MoveListString(newX, newY)
                      };
                        queue.Enqueue([newX, newY]);
                    }
                }
            }

            return [];
        }
    }
}