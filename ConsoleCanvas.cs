namespace TerminalSnake
{
    internal class ConsoleCanvas : I2DCanvas
    {
        private readonly Dictionary<string, object> _canvasObjects = [];
        public ConsoleCanvas()
        {
            Clear();
            FillColor = CanvasColor.Black;
            BrushColor = CanvasColor.Green;
        }

        public int Width => Console.BufferWidth;

        public int Height => Console.BufferHeight;

        public CanvasColor FillColor { get => Console.BackgroundColor.ToCanvasColor(); set => Console.BackgroundColor = value.ToConsoleColor(); }
        public CanvasColor BrushColor { get => Console.ForegroundColor.ToCanvasColor(); set => Console.ForegroundColor = value.ToConsoleColor(); }

        public void Clear()
        {
            Console.Clear();
            _canvasObjects.Clear();
        }

        public void Draw(int x, int y, object? drawable = null)
        {
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

        public object? PixelAt(int x, int y)
        {
            if (_canvasObjects.ContainsKey(MakeKey(x, y)))
                return _canvasObjects[MakeKey(x, y)];
            else
                return null;
        }

        private static string MakeKey(int x, int y) => $"{x},{y}";
    }
}
