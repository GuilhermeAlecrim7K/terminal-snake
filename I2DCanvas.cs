using System.Diagnostics.CodeAnalysis;

namespace TerminalSnake.Canvas
{
    internal enum CanvasColor
    {
        Black = 0,
        DarkBlue = 1,
        DarkGreen = 2,
        DarkCyan = 3,
        DarkRed = 4,
        DarkMagenta = 5,
        DarkYellow = 6,
        Gray = 7,
        DarkGray = 8,
        Blue = 9,
        Green = 10,
        Cyan = 11,
        Red = 12,
        Magenta = 13,
        Yellow = 14,
        White = 15
    }
    internal interface I2DCanvas
    {
        void Draw(int x, int y, object? drawable);
        bool TryGetPixelAt(int x, int y, [NotNullWhen(true)] out object? pixel);
        void Clear();
        int Width { get; }
        int Height { get; }
        CanvasColor FillColor { get; set; }
        CanvasColor BrushColor { get; set; }
    }
}
