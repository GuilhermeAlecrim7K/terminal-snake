namespace TerminalSnake.Objects.Snake
{
    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    internal static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => throw new NotImplementedException($"No opposite configured for {direction}"),
            };
        }
    }
}
