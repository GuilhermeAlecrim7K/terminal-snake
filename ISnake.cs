namespace TerminalSnake.Objects.Snake
{
    internal interface ISnake
    {
        bool TryMove(Direction? direction);
        void Render(int initialSize);
    }
}
