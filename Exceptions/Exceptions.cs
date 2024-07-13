using TerminalSnake.Canvas;

namespace TerminalSnake.Exceptions
{
    internal class CanvasResizedException : Exception
    {
        public CanvasResizedException() : base("Canvas cannot be resized.") { }
    }
    internal class PointOutOfCanvasBoundsException(I2DCanvas canvas, int x, int y, Exception? innerException = null) : Exception(message: $"There was an attempt to perform an operation at coordinates ({x},{y}) in a {canvas.Width}x{canvas.Height} canvas.", innerException: innerException)
    {
    }

    internal class CanvasSizeException(I2DCanvas canvas, int minWidth, int minHeight, Exception? innerException = null) : Exception(message: $"Canvas too small. Expected: {minWidth}x{minHeight} Got: {canvas.Width}x{canvas.Height}", innerException: innerException)
    {
    }
}