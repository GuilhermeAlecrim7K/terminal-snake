using System.Diagnostics.CodeAnalysis;

using TerminalSnake.Exceptions;
using TerminalSnake.Extensions;

namespace TerminalSnake.Canvas
{
    internal class ConsoleCanvas : I2DCanvas
    {
        private readonly Dictionary<string, object> _canvasObjects = [];
        private int _startingWidth;
        private int _startingHeight;

        public ConsoleCanvas()
        {
            Clear();
            _startingWidth = Width;
            _startingHeight = Height;
            FillColor = CanvasColor.Black;
            BrushColor = CanvasColor.Green;
        }

        public int Width => Console.BufferWidth - 1;

        public int Height => Console.BufferHeight - 1;

        public CanvasColor FillColor { get => Console.BackgroundColor.ToCanvasColor(); set => Console.BackgroundColor = value.ToConsoleColor(); }
        public CanvasColor BrushColor { get => Console.ForegroundColor.ToCanvasColor(); set => Console.ForegroundColor = value.ToConsoleColor(); }

        public void Clear()
        {
            Console.Clear();
            _canvasObjects.Clear();
            _startingWidth = Width;
            _startingHeight = Height;
        }

        public void Draw(int x, int y, object? drawable = null)
        {
            if (Resized())
            {
                Clear();
                throw new CanvasResizedException();
            }
            if (x < 0 || y < 0 || x > Width || y > Height)
                throw new PointOutOfCanvasBoundsException(this, x, y);
            Console.SetCursorPosition(x, y);

            if (drawable == null && _canvasObjects.ContainsKey(MakeKey(x, y)))
            {
                _canvasObjects.Remove(MakeKey(x, y));
                Console.Write(" ");
            }
            else if (drawable != null)
            {
                _canvasObjects[MakeKey(x, y)] = drawable;
                Console.Write(drawable);
            }
        }

        public bool TryGetPixelAt(int x, int y, [NotNullWhen(true)] out object? pixel)
        {

            bool result = _canvasObjects.ContainsKey(MakeKey(x, y));
            pixel = result ? _canvasObjects[MakeKey(x, y)] : null;
            return result;
        }

        private static string MakeKey(int x, int y) => $"{x},{y}";

        private bool Resized()
        {
            return _startingHeight != Height || _startingWidth != Width;
        }
    }
}