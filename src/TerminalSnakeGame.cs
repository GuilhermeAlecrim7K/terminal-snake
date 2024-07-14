using TerminalSnake.Canvas;
using TerminalSnake.Exceptions;
using TerminalSnake.Extensions;
using TerminalSnake.GameObjects;

namespace TerminalSnake
{
    internal partial class TerminalSnakeGame
    {
        public TerminalSnakeGame()
        {
            _food = new(_canvas, out _foodChar);
        }

        private Snake? _snake;
        private readonly Food _food;
        private readonly char _foodChar;
        private readonly ConsoleCanvas _canvas = new();
        public void Start()
        {
            Console.CursorVisible = false;
            try
            {
                StartGame();
            }
            catch (Exception ex)
            {
                PrintGameOver(ex.Message);
            }
            finally
            {
                Console.ResetColor();
                Console.CursorVisible = true;
            }
            Console.SetCursorPosition(0, Console.BufferHeight);
            Console.WriteLine();
        }

        private void StartGame()
        {
            PrepareCanvasForTheGame();

            Task<Action> keyListeningTask = ListenToNextInput();
            for (bool endGame = false; !endGame;)
            {
                Direction? direction = null;
                if (keyListeningTask.IsCompleted)
                {
                    Action action = keyListeningTask.Result;
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
                    if (!_snake!.TryMove(ref direction))
                        break;
                    keyListeningTask = ListenToNextInput();
                }
                else
                {
                    endGame = !_snake!.TryMove(ref direction);
                }
                Thread.Sleep(direction == Direction.Up || direction == Direction.Down ? VerticalSpeed : HorizontalSpeed);
            }
            PrintGameOver();
        }

        private static void PrintGameOver(string? message = null)
        {
            const string gameOverMessage = "     Game Over     ";
            int x = Console.BufferWidth / 2 - gameOverMessage.Length / 2;
            int y = Console.BufferHeight / 2;
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(gameOverMessage);

            if (message == null)
                return;

            x = Console.BufferWidth / 2 - message.Length / 2;
            Console.SetCursorPosition(x, ++y);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
        }

        private void PrepareCanvasForTheGame()
        {
            _canvas.Clear();
            if (_canvas.Width < MinPlayableWidth || _canvas.Height < MinPlayableHeight)
                throw new CanvasSizeException(_canvas, MinPlayableWidth, MinPlayableHeight);
            ShowMainMenu();
            RenderSnake();
            _food.Render();
        }

        private void ShowMainMenu()
        {
            const string applicationTitle = "Terminal Snake";
            const string msg = "Press any key to start...";
            if (!TryRenderTitleWithAsciiArt(out int titleY))
            {
                titleY = _canvas.Height / 2 - 1;
                _canvas.Draw(_canvas.Width / 2 - applicationTitle.Length / 2, titleY++, applicationTitle);
            }
            _canvas.Draw(_canvas.Width / 2 - msg.Length / 2, ++titleY, msg);
            Console.ReadKey(intercept: true);
            _canvas.Clear();
        }

        private bool TryRenderTitleWithAsciiArt(out int titleY)
        {
            titleY = -1;
            int maxWidthOfTitle = MainMenuTitle.Split('\n').Max(s => s.Length);
            if (_canvas.Width < maxWidthOfTitle)
                return false;
            int numberOfLinesInTitle = MainMenuTitle.Count(c => c == '\n');
            int titleX = _canvas.Width / 2 - maxWidthOfTitle / 2;
            int totalLines = numberOfLinesInTitle + 2; // 1 space line + 1 text line
            titleY = _canvas.Height / 2 - totalLines;
            foreach (string line in MainMenuTitle.Split('\n'))
            {
                _canvas.Draw(titleX, titleY++, line);
            }
            return true;
        }

        private void RenderSnake()
        {
            _snake = new Snake(_canvas, _foodChar, onEatFood: _food.Render);
            _snake.Render();
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
                    ConsoleKey.UpArrow or ConsoleKey.W or ConsoleKey.K => Action.GoUp,
                    ConsoleKey.DownArrow or ConsoleKey.S or ConsoleKey.J => Action.GoDown,
                    ConsoleKey.RightArrow or ConsoleKey.D or ConsoleKey.L => Action.GoRight,
                    ConsoleKey.LeftArrow or ConsoleKey.A or ConsoleKey.H => Action.GoLeft,
                    _ => Action.DoNothing,
                });

            });
            return result.Task;
        }
    }
}