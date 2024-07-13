using TerminalSnake.Canvas;
using TerminalSnake.GameObjects.Interfaces;

namespace TerminalSnake.GameObjects
{
    internal class Food : IGameObject
    {
        public Food(I2DCanvas canvas, out char foodChar)
        {
            _canvas = canvas;
            foodChar = FoodChar;
        }
        private readonly I2DCanvas _canvas;
        private readonly Random _foodRandom = new();
        private const char FoodChar = '\u0238';
        public void Render()
        {
            int x, y;
            do
            {
                x = _foodRandom.Next(1, _canvas.Width);
                y = _foodRandom.Next(1, _canvas.Height);
            } while (_canvas.TryGetPixelAt(x, y, out _));
            (CanvasColor brushColor, _canvas.BrushColor) = (_canvas.BrushColor, CanvasColor.Red);
            _canvas.Draw(x, y, FoodChar);
            _canvas.BrushColor = brushColor;
        }
    }
}