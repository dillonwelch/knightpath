namespace KnightPath
{
    public static class ChessBoard
    {
        public const int Rows = 8, Colunns = 8;

        public static void ValidatePosition(string position) 
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(position);

            if (position.Length != 2)
            {
              throw new ArgumentException($"Position '{position}' must contain exactly two characters.");
            }

            if (!Char.IsBetween(position[0], 'A', 'H'))
            {
              string positionX = position[0].ToString();
              throw new ArgumentException($"Invalid row identifier '{positionX}' in position '{position}'.");
            }

            int positionY = (int)Char.GetNumericValue(position[1]);
            if (positionY < 1 || positionY >= Rows) 
            {
              throw new ArgumentException($"Invalid column identifier '{positionY}' in position '{position}'.");
            }
        }
    }
}