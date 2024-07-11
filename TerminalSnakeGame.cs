
namespace TerminalSnake
{
    internal class TerminalSnakeGame
    {
        private enum Action
        {
            DoNothing,
            GoUp,
            GoDown,
            GoLeft,
            GoRight,
            Quit,
        }

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

        private async void StartGame()
        {
            PrepareCanvasForTheGame();

            Task<Action> keyListeningTask = ListenToNextInput();
            for (bool endGame = false; !endGame;)
            {
                Direction? direction = null;
                if (keyListeningTask.IsCompleted)
                {
                    Action action = await keyListeningTask;
                    if (action == Action.Quit)
                        break;
                    direction = action switch
                    {
                        Action.GoUp => Direction.Up,
                        Action.GoDown => Direction.Down,
                        Action.GoLeft => Direction.Left,
                        Action.GoRight => Direction.Right,
                        Action.DoNothing => direction,
                        _ => throw new NotImplementedException(),
                    };
                    keyListeningTask = ListenToNextInput();
                }
                try
                {
                    _snake!.Move(direction);
                }
                catch
                {
                    endGame = true;
                }
                Thread.Sleep(90);
            }
            _canvas.Draw(0, _canvas.Height, new string('\u0020', _canvas.Width));
            _canvas.Draw(_canvas.Width / 2 - "Game Over".Length / 2, _canvas.Height, "Game Over");
        }

        private void PrepareCanvasForTheGame()
        {
            _canvas.Clear();
            _snake = new Snake(_canvas);
            _snake.Render(7);
        }

        private static Task<Action> ListenToNextInput()
        {
            TaskCompletionSource<Action> result = new();
            Task.Run(() =>
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(intercept: true);
                result.SetResult(consoleKeyInfo.Key switch
                {
                    ConsoleKey.Q => Action.Quit,
                    ConsoleKey.UpArrow => Action.GoUp,
                    ConsoleKey.DownArrow => Action.GoDown,
                    ConsoleKey.RightArrow => Action.GoRight,
                    ConsoleKey.LeftArrow => Action.GoLeft,
                    ConsoleKey.W => Action.GoUp,
                    ConsoleKey.S => Action.GoDown,
                    ConsoleKey.D => Action.GoRight,
                    ConsoleKey.A => Action.GoLeft,
                    _ => Action.DoNothing,
                });

            });
            return result.Task;
        }
    }
}
