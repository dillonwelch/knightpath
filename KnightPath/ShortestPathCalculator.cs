using static KnightPath.ChessBoard;

namespace KnightPath
{
    public static class ShortestPathCalculator
    {
        static readonly int[] dx = [-2, -1, 1, 2, -2, -1, 1, 2];
        static readonly int[] dy = [-1, -2, -2, -1, 1, 2, 2, 1];

        static string MoveListString(int positionX, int positionY)
        {
            return $"{NumToRow(positionX)}{(positionY + 1)}";
        }

        static bool OnBoard(int positionX, int positionY)
        {
            if (positionX >= 0 && positionY >= 0 && positionX < Rows && positionY < Rows)
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
            ValidatePosition(starting);
            ValidatePosition(ending);

            int startingX = RowToNum(starting[0]); 
            int startingY = (int)(Char.GetNumericValue(starting[1]) - 1);
            int endingX = RowToNum(ending[0]);
            int endingY = (int)(Char.GetNumericValue(ending[1]) - 1);

            var queue = new Queue<int[]>();
            queue.Enqueue([startingX, startingY]);

            Dictionary<int, Dictionary<int, List<string>>> moveList = [];
            for (int row = 0; row < Rows; row++)
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