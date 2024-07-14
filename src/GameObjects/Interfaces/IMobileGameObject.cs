using TerminalSnake.Extensions;

namespace TerminalSnake.GameObjects.Interfaces
{
    internal interface IMobileGameObject : IGameObject
    {
        bool TryMove(ref Direction? direction);
    }
}