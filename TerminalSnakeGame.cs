using TerminalSnake.Canvas;
using TerminalSnake.Exceptions;
using TerminalSnake.Extensions;
using TerminalSnake.GameObjects;
using TerminalSnake.GameObjects.Interfaces;

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

        private const int MinPlayableWidth = 20;
        private const int MinPlayableHeight = 20;
        private const int HorizontalSpeed = 60;
        private const int VerticalSpeed = 120;
        private ISnake? _snake;
        private readonly I2DCanvas _canvas = new ConsoleCanvas();
        private readonly Random _foodRandom = new();
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
                    if (!_snake!.TryMove(direction))
                        break;
                    keyListeningTask = ListenToNextInput();
                }
                else
                {
                    endGame = !_snake!.TryMove(direction);
                }
                // TODO: replace later
                RenderFood();
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
            RenderSnake();
            RenderFood();
        }

        private void RenderFood()
        {
            int x, y;
            do
            {
                x = _foodRandom.Next(1, _canvas.Width);
                y = _foodRandom.Next(1, _canvas.Height);
            } while (_canvas.TryGetPixelAt(x, y, out _));
            _canvas.Draw(x, y, '@');
        }

        private void RenderSnake()
        {
            _snake = new Snake(_canvas, '@');
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
