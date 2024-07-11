namespace KnightPath 
{
  public class ShortestPathCalculator
  {
      static readonly Dictionary<string, int> boardMapping = new Dictionary<string, int>()
      {
          { "A", 0 }, { "B", 1 }, { "C", 2 }, { "D", 3 }, { "E", 4 }, { "F", 5 }, { "G", 6 }, { "H", 7 },
      };
      static readonly List<string> boardMappingKeys = boardMapping.Keys.ToList();
      static readonly int[] dx = [-2, -1, 1, 2, -2, -1, 1, 2];
      static readonly int[] dy = [-1, -2, -2, -1, 1, 2, 2, 1];

      static string MoveListString(int positionX, int positionY)
      {
          return $"{boardMappingKeys[positionX]}{(positionY + 1)}";
      }

      // TODO: Only need public for tests.
      public static bool OnBoard(int positionX, int positionY, int rows, int columns)
      {
          if (positionX >= 0 && positionY >= 0 && positionX < rows && positionY < columns)
          {
              return true;
          }
          else
          {
              return false;
          }
      }

      public static List<string> CalculateShortestPath(string starting, string ending, int rows = 8, int columns = 8)
      {
          int startingX = boardMapping[starting[0].ToString()];
          int startingY = Int32.Parse(starting[1].ToString()) - 1;
          int endingX = boardMapping[ending[0].ToString()];
          int endingY = Int32.Parse(ending[1].ToString()) - 1;
          var queue = new Queue<int[]>();
          queue.Enqueue([startingX, startingY]);

          Dictionary<int, Dictionary<int, List<string>>> moveList = new Dictionary<int, Dictionary<int, List<string>>>();
          for (int row = 0; row < rows; row++)
          {
              moveList[row] = new Dictionary<int, List<string>>();
          }
          moveList[startingX][startingY] = new List<string>() { MoveListString(startingX, startingY) };

          while(queue.Count > 0)
          {
              var current = queue.Dequeue();
              var currentX = current[0];
              var currentY = current[1];

              if (currentX == endingX && currentY == endingY)
              {
                  return moveList[currentX][currentY];
              }

              for(int position = 0; position < 8; position++)
              {
                  var newX = currentX + dx[position];
                  var newY = currentY + dy[position];
                  if(OnBoard(newX, newY, rows, columns) && !moveList[newX].ContainsKey(newY))
                  {
                      moveList[newX][newY] = new List<string>(moveList[currentX][currentY]);
                      moveList[newX][newY].Add(MoveListString(newX, newY));
                      queue.Enqueue([newX, newY]);
                  }
              }
          }

          return new List<string>();
      }
  }
}