namespace TerminalSnake
{
    internal interface ISnake
    {
        void Move(Direction? direction);
        void Render(int initialSize);
    }
}
