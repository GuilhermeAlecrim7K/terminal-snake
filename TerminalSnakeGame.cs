
namespace TerminalSnake
{
    internal class TerminalSnakeGame
    {
        private const int MinPlayableWidth = 480;
        private const int MinPlayableHeight = 480;
        private const int horizontalSpeed = 30;
        private const int verticalSpeed = 60;
        private ISnake? _snake;
        private readonly I2DCanvas _canvas = new ConsoleCanvas();
        public void Start()
        {
            Console.CursorVisible = false;
            StartGame();
        }

        private void StartGame()
        {
            PrepareCanvasForTheGame();
        }

        private void PrepareCanvasForTheGame()
        {
            _canvas.Clear();
            _snake = new Snake(_canvas);
            _snake.Render(3);
        }
    }
}
