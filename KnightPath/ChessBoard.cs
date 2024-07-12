namespace KnightPath
{
    public static class ChessBoard
    {
        public const int Rows = 8,
            Colunns = 8;

        public static void ValidatePosition(string position)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(position);

            // NOTE: After looking at this code with fresh eyes, I think I could probably do this
            // much more simply with a regex! However this would lose some fidelity in the level
            // of detail of the error.
            if (position.Length != 2)
            {
                throw new ArgumentException(
                    $"Position '{position}' must contain exactly two characters."
                );
            }

            if (!Char.IsBetween(position[0], 'A', 'H'))
            {
                string positionX = position[0].ToString();
                throw new ArgumentException(
                    $"Invalid row identifier '{positionX}' in position '{position}'."
                );
            }

            int positionY = (int)Char.GetNumericValue(position[1]);
            if (positionY < 1 || positionY >= Rows)
            {
                throw new ArgumentException(
                    $"Invalid column identifier '{positionY}' in position '{position}'."
                );
            }
        }
    }
}
