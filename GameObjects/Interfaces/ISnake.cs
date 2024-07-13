using TerminalSnake.Extensions;

namespace TerminalSnake.GameObjects.Interfaces
{
    internal interface ISnake
    {
        bool TryMove(Direction? direction);
        void Render(int initialSize);
    }
}