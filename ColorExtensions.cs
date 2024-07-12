using TerminalSnake.Canvas;

namespace TerminalSnake.Extensions
{
    internal static class ColorExtensions
    {
        public static CanvasColor ToCanvasColor(this ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => CanvasColor.Black,
                ConsoleColor.DarkBlue => CanvasColor.DarkBlue,
                ConsoleColor.DarkGreen => CanvasColor.DarkGreen,
                ConsoleColor.DarkCyan => CanvasColor.DarkCyan,
                ConsoleColor.DarkRed => CanvasColor.DarkRed,
                ConsoleColor.DarkMagenta => CanvasColor.DarkMagenta,
                ConsoleColor.DarkYellow => CanvasColor.DarkYellow,
                ConsoleColor.Gray => CanvasColor.Gray,
                ConsoleColor.DarkGray => CanvasColor.DarkGray,
                ConsoleColor.Blue => CanvasColor.Blue,
                ConsoleColor.Green => CanvasColor.Green,
                ConsoleColor.Cyan => CanvasColor.Cyan,
                ConsoleColor.Red => CanvasColor.Red,
                ConsoleColor.Magenta => CanvasColor.Magenta,
                ConsoleColor.Yellow => CanvasColor.Yellow,
                ConsoleColor.White => CanvasColor.White,
                _ => throw new NotImplementedException(),
            };
        }
        public static ConsoleColor ToConsoleColor(this CanvasColor color)
        {
            return color switch
            {
                CanvasColor.Black => ConsoleColor.Black,
                CanvasColor.DarkBlue => ConsoleColor.DarkBlue,
                CanvasColor.DarkGreen => ConsoleColor.DarkGreen,
                CanvasColor.DarkCyan => ConsoleColor.DarkCyan,
                CanvasColor.DarkRed => ConsoleColor.DarkRed,
                CanvasColor.DarkMagenta => ConsoleColor.DarkMagenta,
                CanvasColor.DarkYellow => ConsoleColor.DarkYellow,
                CanvasColor.Gray => ConsoleColor.Gray,
                CanvasColor.DarkGray => ConsoleColor.DarkGray,
                CanvasColor.Blue => ConsoleColor.Blue,
                CanvasColor.Green => ConsoleColor.Green,
                CanvasColor.Cyan => ConsoleColor.Cyan,
                CanvasColor.Red => ConsoleColor.Red,
                CanvasColor.Magenta => ConsoleColor.Magenta,
                CanvasColor.Yellow => ConsoleColor.Yellow,
                CanvasColor.White => ConsoleColor.White,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
